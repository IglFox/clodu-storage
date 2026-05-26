import jwt
from app.core.config import settings


def verify_token(token: str):
    try:
        return jwt.decode(
            token, settings.JWT_SECRET_KEY, algorithms=[settings.JWT_ALGORITHM]
        )
    except Exception:
        return None
