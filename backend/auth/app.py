from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from schemas.auth import LoginRequest, TokenResponse, VerifyResponse, VerifyRequest

app = FastAPI()

VALID_TOKEN = "hardcoded-token-123"


@app.post("/login", response_model=TokenResponse)
async def login(req: LoginRequest):
    if not req.username or not req.password:
        raise HTTPException(400, "Username and password required")
    return TokenResponse(token=VALID_TOKEN)


@app.post("/verify", response_model=VerifyResponse)
async def verify(req: VerifyRequest):
    return VerifyResponse(valid=(req.token == VALID_TOKEN))
