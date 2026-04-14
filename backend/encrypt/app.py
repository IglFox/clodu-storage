from fastapi import FastAPI, UploadFile, File
from fastapi.responses import Response

app = FastAPI()
PREFIX = b"ENCRYPTED:"


@app.post("/encrypt")
async def encrypt(data: UploadFile = File(...)):
    content = await data.read()
    encrypted = PREFIX + content
    return Response(content=encrypted, media_type="application/octet-stream")


@app.post("/decrypt")
async def decrypt(data: UploadFile = File(...)):
    content = await data.read()
    if content.startswith(PREFIX):
        decrypted = content[len(PREFIX) :]
    else:
        decrypted = content
    return Response(content=decrypted, media_type="application/octet-stream")
