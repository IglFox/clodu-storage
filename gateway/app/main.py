from fastapi import FastAPI, Depends, Request
from fastapi.responses import Response
from app.dependencies import get_token_header
from app.clients.registry import get_storage_client, get_analysis_client, get_encrypt_client, get_auth_client

app = FastAPI()

@app.api_route("/auth/{path:path}", methods=["POST"])
async def proxy_auth(request: Request, client = Depends(get_auth_client)):
    resp = await client.proxy_request(request, request.path_params["path"])
    return Response(content=resp.content, status_code=resp.status_code, headers=dict(resp.headers))

@app.api_route("/storage/{path:path}", methods=["GET", "POST"])
async def proxy_storage(request: Request, token = Depends(get_token_header), client = Depends(get_storage_client)):
    resp = await client.proxy_request(request, request.path_params["path"])
    return Response(content=resp.content, status_code=resp.status_code, headers=dict(resp.headers))

@app.api_route("/analysis/{path:path}", methods=["POST"])
async def proxy_analysis(request: Request, token = Depends(get_token_header), client = Depends(get_analysis_client)):
    resp = await client.proxy_request(request, request.path_params["path"])
    return Response(content=resp.content, status_code=resp.status_code, headers=dict(resp.headers))

@app.api_route("/encrypt/{path:path}", methods=["POST"])
async def proxy_encrypt(request: Request, token = Depends(get_token_header), client = Depends(get_encrypt_client)):
    resp = await client.proxy_request(request, request.path_params["path"])
    return Response(content=resp.content, status_code=resp.status_code, headers=dict(resp.headers))
