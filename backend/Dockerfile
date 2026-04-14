FROM python:3.12-slim

ARG SERVICE_NAME
ENV SERVICE_NAME=${SERVICE_NAME}

WORKDIR /app

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Копируем конкретную папку сервиса
COPY ${SERVICE_NAME}/app.py ./app.py
COPY schemas ./schemas
EXPOSE ${PORT}

CMD sh -c "uvicorn app:app --host 0.0.0.0 --port $PORT"
