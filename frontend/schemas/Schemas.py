from pydantic import BaseModel, Field


class User(BaseModel):
    username: str = Field(..., title="User's username")
    password: str = Field(..., title="User's password")

