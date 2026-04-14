from pydantic import BaseModel


class LoginRequest(BaseModel):
    username: str
    password: str


class TokenResponse(BaseModel):
    token: str
    token_type: str = "bearer"


class VerifyResponse(BaseModel):
    valid: bool


class VerifyRequest(BaseModel):
    token: str
