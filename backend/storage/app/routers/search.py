from fastapi import APIRouter, Depends, Query
from app.dependencies import get_es
from app.services.search_service import search_docs

router = APIRouter(prefix="/storage/search", tags=["search"])


@router.get("")
async def fulltext_search(
    q: str = Query(..., min_length=1, description="Поисковый запрос"),
    page: int = Query(1, ge=1),
    size: int = Query(20, ge=1, le=100),
    es=Depends(get_es),
):
    results = await search_docs(es, q, page, size)
    return results
