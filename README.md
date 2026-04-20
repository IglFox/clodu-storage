## 🛠️ Установка и первый запуск

### Требования

- Ubuntu 22.04+ / Debian 12+ (или WSL2 для Windows)
- Rust 1.85+
- SQLite 3
- curl, jq (для тестирования API)

### Установка Rust

```bash
# Установка curl
sudo apt update && sudo apt install curl -y

# Установка Rust (официальный способ)
curl --proto '=https' --tlsv1.2 -sSf https://sh.rustup.rs | sh

# Применить изменения окружения
source "$HOME/.cargo/env"

# Проверить версию
cargo --version

Функционал makefile:
# Показать все команды
make help

# Запустить auth-service
make dev-auth

# Запустить с подробными логами
make dev-auth-debug

# Посмотреть содержимое БД
make db-view

# Сбросить БД (удалить всех пользователей)
make db-reset

# Очистить артефакты сборки
make clean

# Собрать релизную версию
make build-auth

# Протестировать API (сервер должен быть запущен в другом терминале)
make test-auth

# Установить утилиты (если чего-то не хватает)
make install-tools


Общий пример использования cargo:
cd backend/auth
cargo build

В другом терминале:
//Для регистрации юзера
curl -X POST http://localhost:8081/auth/register  -H "Content-Type: application/json"  -d '{"email": "user@example.com", "password": "mypassword123"}'
Успешное создание пользователя:
{
  "id": "92653487-92ad-437b-b50d-419818e41806",
  "email": "user@example.com"
}
Пользователь уже существует:
{
  "error": "User already exists"
}


//Для входа в существующий аккаунт(Получение JWT токена на 24 часа)
curl -X POST http://localhost:8081/auth/login -H "Content-Type: application/json" -d '{"email": "user@example.com", "password": "mypassword123"}'
Успешное создание токена:
{
  "access_token": "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9...",
  "token_type": "bearer"
}
Неверные данные входа:
{
  "error": "Invalid email or password"
}


Запрос на получение профиля в защищеном эндпоинте:
curl -X GET http://localhost:8081/auth/me -H "Authorization: Bearer ВСТАВЬ_СЮДА_ТОКЕН"
Успех:
{
  "id": "92653487-92ad-437b-b50d-419818e41806",
  "email": "user@example.com"
}
Ответ без токена:
{
  "error": "Missing authorization header"
}
Невалидный токен:
{
  "error": "Invalid or expired token"
}


Просмотр бд юзеров:
cd backend/auth
sqlite3 auth.db "SELECT id, email, created_at FROM users;"