from fastapi import FastAPI
from routers import storage  # импортируем модуль с роутером

app = FastAPI(title="Cloud Storage")

# Подключаем роутер
app.include_router(storage.router)


# Можно также добавить корневой маршрут
@app.get("/")
def root():
    return {"message": "Cloud Storage API"}
