from pydantic_settings import BaseSettings, SettingsConfigDict

class Settings(BaseSettings):
    JWT_SECRET_KEY: str
    JWT_ALGORITHM: str = "HS256"
    
    AUTH_URL: str
    STORAGE_URL: str
    ANALYSIS_URL: str
    ENCRYPT_URL: str
    
    model_config = SettingsConfigDict(env_file=".env", extra="ignore")

settings = Settings()
