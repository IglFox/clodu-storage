from fastapi import APIRouter, UploadFile, File, Depends, HTTPException, Query
from fastapi.responses import FileResponse
from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy import select, func
from typing import List
from app.dependencies import get_db, get_es
from app.models.db import FileMetadata
from app.services.file_service import save_file, delete_file_meta
from pydantic import BaseModel
import os

router = APIRouter(prefix="/storage/files", tags=["files"])


class FileOut(BaseModel):
    file_id: str
    original_filename: str
    content_type: str | None
    size: int
    uploaded_at: str

    class Config:
        from_attributes = True


@router.post("", response_model=FileOut)
async def upload_file(
    file: UploadFile = File(...), db: AsyncSession = Depends(get_db), es=Depends(get_es)
):
    if file.size and file.size > 50 * 1024 * 1024:
        raise HTTPException(400, "Файл слишком большой")
    meta = await save_file(file, db, es)
    return FileOut(
        file_id=str(meta.file_id),
        original_filename=meta.original_filename,
        content_type=meta.content_type,
        size=meta.size,
        uploaded_at=meta.uploaded_at.isoformat(),
    )


@router.get("", response_model=dict)
async def list_files(
    skip: int = Query(0, ge=0),
    limit: int = Query(20, ge=1, le=100),
    db: AsyncSession = Depends(get_db),
):
    stmt = (
        select(FileMetadata)
        .order_by(FileMetadata.uploaded_at.desc())
        .offset(skip)
        .limit(limit)
    )
    total = await db.scalar(select(func.count()).select_from(FileMetadata))
    result = await db.execute(stmt)
    items = result.scalars().all()
    return {
        "total": total,
        "items": [
            FileOut(
                file_id=str(f.file_id),
                original_filename=f.original_filename,
                content_type=f.content_type,
                size=f.size,
                uploaded_at=f.uploaded_at.isoformat(),
            )
            for f in items
        ],
    }


@router.get("/{file_id}")
async def download_file(file_id: str, db: AsyncSession = Depends(get_db)):
    stmt = select(FileMetadata).where(FileMetadata.file_id == file_id)
    result = await db.execute(stmt)
    meta = result.scalar_one_or_none()
    if not meta:
        raise HTTPException(404, "Файл не найден")
    if not os.path.exists(meta.storage_path):
        raise HTTPException(404, "Файл на диске отсутствует")
    return FileResponse(
        path=meta.storage_path,
        filename=meta.original_filename,
        media_type=meta.content_type,
    )


@router.delete("/{file_id}")
async def remove_file(
    file_id: str, db: AsyncSession = Depends(get_db), es=Depends(get_es)
):
    meta = await delete_file_meta(file_id, db, es)
    if not meta:
        raise HTTPException(404, "Файл не найден")
    return {"detail": "Файл удалён"}
