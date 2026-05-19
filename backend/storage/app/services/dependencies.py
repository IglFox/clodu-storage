from elasticsearch import AsyncElasticsearch
from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession, async_sessionmaker
from app.config import settings
from app.services.search_service import MAPPING, INDEX_NAME

engine = create_async_engine(settings.database_url, echo=False)
async_session = async_sessionmaker(engine, class_=AsyncSession, expire_on_commit=False)

es_client = AsyncElasticsearch(settings.elasticsearch_url)


async def get_db():
    async with async_session() as session:
        yield session


async def get_es():
    # Проверка и создание индекса при первом обращении
    if not await es_client.indices.exists(index=INDEX_NAME):
        await es_client.indices.create(index=INDEX_NAME, body=MAPPING)
    return es_client
