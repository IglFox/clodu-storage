import jwt
from jwt import ExpiredSignatureError, InvalidTokenError
from app.core.config import settings

def verify_token(token: str):
    try:
        return jwt.decode(token, settings.JWT_SECRET_KEY, algorithms=[settings.JWT_ALGORITHM])
    except ExpiredSignatureError:
        # Handle expired token specifically
        return None
    except InvalidTokenError:
        # Handle invalid token specifically
        return None
