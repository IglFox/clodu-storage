# clodu-storage

Современный прототип облачного хранилища с веб‑интерфейсом, авторизацией и шлюзом, разбитый на несколько сервисов (`backend`, `frontend`, `gateway`).[page:1] Проект собирается и запускается с помощью `docker-compose` и `Makefile`.[page:1]

## Возможности

- Регистрация и авторизация пользователей (auth‑сервис в backend).
- Веб‑интерфейс для входа и работы с хранилищем (frontend на HTML/Vue/JS).[page:1]
- API‑шлюз (`gateway`) для проксирования запросов к backend.[page:1]
- Тестовый UI и e2e‑тесты для фронтенда (папка `tests/frontend`).[page:1]

> Часть функционала находится в активной разработке и может меняться.

## Стек технологий

- **Frontend:** HTML, Vue, JavaScript, CSS.[page:1]
- **Backend:** Python (см. `pyproject.toml`).[page:1]
- **Gateway:** Rust.[page:1]
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

Предполагается, что установлены Docker и Docker Compose.

```bash
# Клонировать репозиторий
git clone https://github.com/IglFox/clodu-storage.git
cd clodu-storage

# Собрать и поднять все сервисы
docker compose up --build
```

После запуска:

- Backend и gateway будут подняты внутри Docker‑сети (см. `docker-compose.yaml`).[page:1]
- Frontend будет доступен по адресу, указанному в `docker-compose.yaml` (например, `http://localhost:XXXX`).  
  Уточни порт и при необходимости впиши его сюда.

Остановка сервисов:

```bash
docker compose down
```

## Запуск для разработки

Актуальные команды следует смотреть в `Makefile`.[page:1]

Примеры (если используешь такие цели — отредактируй под себя):

```bash
# Запуск backend в dev-режиме
make backend-dev

# Запуск frontend
make frontend-dev

# Запуск gateway
make gateway-dev

# Прогон тестов
make test
```

## Тестирование

Фронтенд‑тесты и экспериментальные сценарии лежат в `tests/frontend`.[page:1]

Пример (замени на реальные команды):

```bash
# Запуск фронтенд-тестов
make test-frontend
```

## Планы по развитию

- Реализовать полноценное хранение и управление файлами.
- Добавить загрузку/скачивание, папки, квоты.
- Расширить покрытие тестами backend и gateway.
- Настроить CI/CD (GitHub Actions) для линтинга, тестов и сборки образов.[page:1]

## Лицензия

Проект распространяется под лицензией MIT, текст лицензии см. в файле `LICENSE`.[page:1]
