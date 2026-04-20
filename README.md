# clodu-storage

Современный прототип облачного хранилища с веб‑интерфейсом, авторизацией и шлюзом, разбитый на несколько сервисов (`backend`, `frontend`, `gateway`).[page:1] Проект собирается и запускается с помощью `docker-compose` и `Makefile`.[page:1]

## Возможности

- Регистрация и авторизация пользователей (auth‑сервис в backend).
- Веб‑интерфейс для входа и работы с хранилищем (frontend на HTML/Vue/JS).
- API‑шлюз (`gateway`) для проксирования запросов к backend.

> Часть функционала находится в активной разработке и может меняться.

## Стек технологий

- **Frontend:** HTML, Vue, JavaScript, CSS.[page:1]
- **Backend:** Python, Rust (см. `pyproject.toml`).[page:1]
- **Gateway:** Python.[page:1]
- **Инфраструктура:** Docker, Docker Compose, Makefile.[page:1]

## Структура репозитория

```text
backend/          # backend-сервисы (авторизация, бизнес-логика)
frontend/         # клиентское веб-приложение
gateway/          # API-шлюз
tests/frontend/   # тесты и экспериментальный UI для фронта
docker-compose.yaml
Makefile
pyproject.toml
LICENSE
проект.md         # описание и внутренний документ по проекту
test.html         # тестовая страница для фронта
```

## Быстрый старт (через Docker)


```bash
# Клонировать репозиторий
git clone https://github.com/IglFox/clodu-storage.git
cd clodu-storage

# Собрать и поднять все сервисы
make install
make run
```

or

```bash
make install
make build-auth
make run-auth
```

После запуска:

- Backend и gateway будут подняты внутри Docker‑сети (см. `docker-compose.yaml`).[page:1]
- Frontend будет доступен по адресу, указанному в `docker-compose.yaml` (`http://localhost:3000`).  

Остановка сервисов:

```bash
docker compose down
```



## Лицензия

Проект распространяется под лицензией MIT, текст лицензии см. в файле `LICENSE`.[page:1]
