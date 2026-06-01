import logging
from fastapi import FastAPI, Depends, Request
from fastapi.responses import Response
from fastapi.middleware.cors import CORSMiddleware
from app.dependencies import get_token_header
from app.clients.registry import (
    get_storage_client,
    get_analysis_client,
    get_encrypt_client,
    get_auth_client,
)

logging.basicConfig(level=logging.WARNING)
logger = logging.getLogger(__name__)

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Словарь для маппинга префиксов API на клиентов
CLIENT_MAP = {
    "/api/Auth": get_auth_client,
    "/api/Files": get_storage_client,
    "/api/Analysis": get_analysis_client,
    "/api/Encrypt": get_encrypt_client,
}


@app.api_route("/{full_path:path}", methods=["GET", "POST", "DELETE", "PUT", "OPTIONS"])
async def catch_all(request: Request):
    full_path = request.url.path

    # Handle CORS preflight
    if request.method == "OPTIONS":
        return Response(status_code=200)

    for prefix, get_client in CLIENT_MAP.items():
        if full_path.startswith(prefix):
            client = get_client()
            resp = await client.proxy_request(request, full_path)

            # Фильтруем заголовки, чтобы избежать конфликтов (например, Content-Length + Transfer-Encoding)
            excluded_headers = {
                "content-length",
                "transfer-encoding",
                "connection",
                "keep-alive",
                "date",
                "server",
            }
            filtered_headers = {
                k: v
                for k, v in resp.headers.items()
                if k.lower() not in excluded_headers
            }

            return Response(
                content=resp.content,
                status_code=resp.status_code,
                headers=filtered_headers,
            )

    return {"error": "Service not found"}, 404
