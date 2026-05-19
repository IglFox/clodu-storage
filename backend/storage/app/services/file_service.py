import uuid
import os
import aiofiles
from fastapi import UploadFile
from sqlalchemy.ext.asyncio import AsyncSession
from app.models.db import FileMetadata
from app.config import settings
from app.services.text_extraction import extract_text
from app.services.search_service import index_document, delete_document


async def save_file(upload_file: UploadFile, db: AsyncSession, es):
    file_id = uuid.uuid4()
    ext = os.path.splitext(upload_file.filename)[1]
    storage_filename = f"{file_id}{ext}"
    storage_path = os.path.join(settings.storage_dir, storage_filename)

    # Сохраняем файл на диск
    content = await upload_file.read()
    os.makedirs(settings.storage_dir, exist_ok=True)
    async with aiofiles.open(storage_path, "wb") as f:
        await f.write(content)

    # Извлекаем текст
    text = await extract_text(storage_path, upload_file.content_type)

    # Метаданные в БД
    meta = FileMetadata(
        file_id=file_id,
        original_filename=upload_file.filename,
        storage_path=storage_path,
        content_type=upload_file.content_type,
        size=len(content),
        extracted_text=text[:10000],  # сохраняем начало для быстрого отображения
    )
    db.add(meta)
    await db.commit()
    await db.refresh(meta)

    # Индексация в ES (не блокируем ответ)
    await index_document(
        es, str(file_id), upload_file.filename, upload_file.content_type, text
    )

    return meta


async def delete_file_meta(file_id: str, db: AsyncSession, es):
    from sqlalchemy import select

    stmt = select(FileMetadata).where(FileMetadata.file_id == file_id)
    result = await db.execute(stmt)
    meta = result.scalar_one_or_none()
    if not meta:
        return None
    # Удаляем файл с диска
    try:
        os.remove(meta.storage_path)
    except OSError:
        pass
    # Удаляем из БД
    await db.delete(meta)
    await db.commit()
    # Удаляем из ES
    await delete_document(es, file_id)
    return meta
