import httpx
import logging
from fastapi import Request

logger = logging.getLogger(__name__)


class AsyncGatewayClient:
    def __init__(self, base_url: str):
        self.base_url = base_url
        self.client = httpx.AsyncClient(base_url=base_url)

    async def proxy_request(self, request: Request, path: str):
        clean_path = path.lstrip("/")

        query = request.query_params

        # Proxy request
        response = await self.client.request(
            method=request.method,
            url=clean_path,
            params=query,
            headers={
                k: v
                for k, v in request.headers.items()
                if k.lower() not in ("host", "connection", "content-length")
            },
            content=await request.body(),
        )
        return response
