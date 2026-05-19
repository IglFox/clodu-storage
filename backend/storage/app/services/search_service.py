INDEX_NAME = "documents"
MAPPING = {
    "settings": {
        "analysis": {
            "analyzer": {
                "russian_english": {
                    "type": "custom",
                    "tokenizer": "standard",
                    "filter": ["lowercase", "russian_morphology", "english_morphology"],
                }
            },
            "filter": {
                "russian_morphology": {"type": "hunspell", "locale": "ru_RU"},
                "english_morphology": {"type": "hunspell", "locale": "en_US"},
            },
        }
    },
    "mappings": {
        "properties": {
            "file_id": {"type": "keyword"},
            "original_filename": {"type": "keyword"},
            "content_type": {"type": "keyword"},
            "content": {"type": "text", "analyzer": "russian_english"},
        }
    },
}


async def index_document(
    es, file_id: str, original_filename: str, content_type: str, text: str
):
    await es.index(
        index=INDEX_NAME,
        id=file_id,
        body={
            "file_id": file_id,
            "original_filename": original_filename,
            "content_type": content_type,
            "content": text,
        },
    )


async def delete_document(es, file_id: str):
    try:
        await es.delete(index=INDEX_NAME, id=file_id)
    except Exception:
        pass


async def search_docs(es, query: str, page: int = 1, size: int = 20):
    from_ = (page - 1) * size
    body = {
        "query": {
            "multi_match": {
                "query": query,
                "fields": ["original_filename^3", "content"],
            }
        },
        "highlight": {
            "fields": {"content": {"fragment_size": 150, "number_of_fragments": 3}}
        },
        "from": from_,
        "size": size,
    }
    resp = await es.search(index=INDEX_NAME, body=body)
    hits = resp["hits"]["hits"]
    total = resp["hits"]["total"]["value"]

    results = []
    for hit in hits:
        src = hit["_source"]
        highlights = hit.get("highlight", {}).get("content", [])
        results.append({
            "file_id": src["file_id"],
            "filename": src["original_filename"],
            "score": hit["_score"],
            "snippets": highlights,
        })
    return {"total": total, "items": results, "page": page, "size": size}
