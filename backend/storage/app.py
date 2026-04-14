from fastapi import FastAPI, UploadFile, File, HTTPException
from fastapi.responses import Response
from schemas.storage import FileInfo, FileUploadResponse
from typing import List, Dict
import uuid

app = FastAPI()
storage: Dict[str, Dict] = {}


@app.post("/upload", response_model=FileUploadResponse)
async def upload(file: UploadFile = File(...)):
    fid = str(uuid.uuid4())
    content = await file.read()
    storage[fid] = {"filename": file.filename, "content": content}
    return FileUploadResponse(id=fid, filename=file.filename)


@app.get("/list", response_model=List[FileInfo])
async def list_files():
    return [FileInfo(id=k, filename=v["filename"]) for k, v in storage.items()]


@app.get("/download/{file_id}")
async def download(file_id: str):
    if file_id not in storage:
        raise HTTPException(404, "File not found")
    return Response(
        content=storage[file_id]["content"], media_type="application/octet-stream"
    )
