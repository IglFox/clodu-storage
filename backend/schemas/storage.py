from pydantic import BaseModel
from typing import List


class FileInfo(BaseModel):
    id: str
    filename: str


class FileUploadResponse(BaseModel):
    id: str
    filename: str


class FileListResponse(BaseModel):
    files: List[FileInfo]
