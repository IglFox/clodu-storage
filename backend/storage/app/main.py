from fastapi import FastAPI
from app.routers import files, search

app = FastAPI(title="File Storage & Search API", version="1.0.0")
app.include_router(files.router)
app.include_router(search.router)


@app.on_event("startup")
async def startup():
    # Инициализация БД (создание таблиц) – в dependencies при первом вызове ES тоже создаётся индекс
    from app.models.db import Base
    from app.dependencies import engine

    async with engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all)
