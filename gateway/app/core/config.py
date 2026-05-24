from pydantic_settings import BaseSettings, SettingsConfigDict

class Settings(BaseSettings):
    JWT_SECRET_KEY: str
    JWT_ALGORITHM: str = "HS256"
    AUTH_SERVICE_URL: str
    STORAGE_SERVICE_URL: str
    ANALYSIS_SERVICE_URL: str
    ENCRYPT_SERVICE_URL: str

    model_config = SettingsConfigDict(env_file=".env")

settings = Settings()
