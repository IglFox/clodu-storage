from pydantic import BaseModel


class EncryptResponse(BaseModel):
    encrypted_data: bytes  # в реальности bytes, но pydantic требует Base64? Лучше str
    # Для простоты оставим bytes, но при передаче по HTTP нужно будет кодировать.
    # В заглушках можно просто вернуть Response с raw bytes, а не JSON.
    # Поэтому модели для encrypt могут быть не нужны, используем raw response.
