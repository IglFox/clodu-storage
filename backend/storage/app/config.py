from pydantic_settings import BaseSettings


class Settings(BaseSettings):
    database_url: str = "postgresql+asyncpg://user:password@db:5432/filestorage"
    elasticsearch_url: str = "http://elasticsearch:9200"
    storage_dir: str = "/data/files"
    max_file_size_mb: int = 50  # ограничение при загрузке

    class Config:
        env_file = ".env"


settings = Settings()
