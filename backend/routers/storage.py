from fastapi import APIRouter, Depends, HTTPException
from typing import List

# Создаём экземпляр роутера
router = APIRouter(
    prefix="/files",  # все пути этого роутера будут начинаться с /files
    tags=["files"],  # группировка в документации Swagger
)


# Заглушка зависимости (позже замените на реальную)
def get_current_user():
    return {"username": "alice"}


@router.get("/")
async def list_files(user=Depends(get_current_user)):
    """GET /files/ — список файлов пользователя"""
    return [{"id": 1, "name": "doc.txt"}, {"id": 2, "name": "photo.jpg"}]


@router.post("/upload")
async def upload_file(user=Depends(get_current_user)):
    """POST /files/upload — загрузка файла"""
    return {"status": "uploaded"}


@router.get("/{file_id}")
async def get_file(file_id: int, user=Depends(get_current_user)):
    """GET /files/{file_id} — получить метаданные файла"""
    return {"id": file_id, "name": f"file_{file_id}.bin"}
