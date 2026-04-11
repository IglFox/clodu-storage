FROM python:3.11-slim

WORKDIR /app

RUN pip install fastapi uvicorn

COPY . .

ENTRYPOINT ["sh", "-c", "uvicorn routers.${ROUTER_NAME}:app --host 0.0.0.0 --port 8000"]
