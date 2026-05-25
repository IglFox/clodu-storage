from pydantic import BaseModel


class LoginRequest(BaseModel):
    email: str
    password: str


class RegisterRequest(BaseModel):
    email: str
    password: str
    username: str


class TokenResponse(BaseModel):
    token: str
    token_type: str = "bearer"


class VerifyResponse(BaseModel):
    valid: bool


class VerifyRequest(BaseModel):
    token: str
