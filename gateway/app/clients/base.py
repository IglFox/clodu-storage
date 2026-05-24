import httpx
from fastapi import Request

class AsyncGatewayClient:
    def __init__(self, base_url: str):
        self.base_url = base_url
        self.client = httpx.AsyncClient(base_url=base_url)

    async def proxy_request(self, request: Request, path: str):
        url = f"{path}"
        query = request.query_params
        
        # Proxy request
        response = await self.client.request(
            method=request.method,
            url=url,
            params=query,
            headers={k: v for k, v in request.headers.items() if k.lower() not in ("host", "connection", "content-length")},
            content=await request.body()
        )
        return response
