from venv import logger
from fastapi import FastAPI, Request, HTTPException, Depends, Header
from fastapi.responses import Response
from fastapi.middleware.cors import CORSMiddleware
from urllib import request, parse
import os
import logging

logger = logging.getLogger(__name__)

app = FastAPI()
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:5173"],  # можно заменить на ["*"] для всех
    allow_credentials=True,
    allow_methods=["*"],  # GET, POST, PUT, DELETE, OPTIONS и т.д.
    allow_headers=["*"],  # любые пользовательские заголовки
)
SERVICES = {
    "auth": os.getenv("AUTH_URL", "http://auth:8081"),
    "storage": os.getenv("STORAGE_URL", "http://storage:8002"),
    "analysis": os.getenv("ANALYSIS_URL", "http://analysis:8003"),
    "encrypt": os.getenv("ENCRYPT_URL", "http://encrypt:8004"),
}

# Фиксированный токен (для заглушки)
JWT_SECRET_KEY = os.getenv("JWT_SECRET_KEY")


def verify_token(authorization: str = Header(...)):
    """Извлекает токен из заголовка Authorization: Bearer <token>"""
    if not authorization.startswith("Bearer "):
        raise HTTPException(status_code=401, detail="Invalid token format")
    token = authorization.split(" ")[1]
    if token != JWT_SECRET_KEY:
        raise HTTPException(status_code=401, detail="Invalid token")
    return token


def proxy_sync(target: str, method: str, headers: dict, body: bytes, params: dict):
    logger.warning(f"{method} body={body}, headers={headers}")
    url = target
    if params:
        url += "?" + parse.urlencode(params)
    req = request.Request(url, data=body if body else None, method=method)
    # Проксируем все заголовки, кроме host и connection
    for k, v in headers.items():
        if k.lower() not in ("host", "connection", "content-length"):
            req.add_header(k, v)
    with request.urlopen(req) as resp:
        content = resp.read()
        status = resp.status
        resp_headers = dict(resp.headers)
    return Response(content=content, status_code=status, headers=resp_headers)


# Эндпоинты, не требующие токена
@app.api_route("/auth/{path:path}", methods=["POST"])
async def auth_proxy(request: Request):
    target = SERVICES["auth"] + "/auth/" + request.path_params["path"]
    logger.warning(target)
    body = await request.body()
    return proxy_sync(
        target, request.method, dict(request.headers), body, dict(request.query_params)
    )


# Эндпоинты, требующие токен (защищённые)
@app.api_route("/storage/{path:path}", methods=["GET", "POST"])
async def storage_proxy(request: Request, token: str = Depends(verify_token)):
    target = SERVICES["storage"] + "/storage/" + request.path_params["path"]
    body = await request.body()
    return proxy_sync(
        target, request.method, dict(request.headers), body, dict(request.query_params)
    )


@app.api_route("/analysis/{path:path}", methods=["POST"])
async def analysis_proxy(request: Request, token: str = Depends(verify_token)):
    target = SERVICES["analysis"] + "/analysis/" + request.path_params["path"]
    body = await request.body()
    return proxy_sync(
        target, request.method, dict(request.headers), body, dict(request.query_params)
    )


@app.api_route("/encrypt/{path:path}", methods=["POST"])
async def encrypt_proxy(request: Request, token: str = Depends(verify_token)):
    target = SERVICES["encrypt"] + "/encrypt/" + request.path_params["path"]
    body = await request.body()
    return proxy_sync(
        target, request.method, dict(request.headers), body, dict(request.query_params)
    )
