# ============================================
# 🛠️  Frontend Makefile для Vite/Vue/React
# ============================================
# Используй:
#   make dev     - запустить локальный сервер разработки
#   make build   - собрать для production
#   make preview - просмотреть production-сборку локально
#   make lint    - проверить код на ошибки (если настроен)
#   make clean   - удалить папку dist
# ============================================

# Папка с исходниками
SRC_DIR = src
# Основной входной файл (подбираемся к структуре из auth.js)
# Так как у тебя хранилище в src/stores/auth.js, логично, что приложение стартует в src/main.js
MAIN_JS = $(SRC_DIR)/main.js

# ============================================
# 1. Основная цель для разработки
# ============================================
.PHONY: dev
dev:
	@echo "🚀 Запускаем dev-сервер с HMR..."
	@cd frontend && vite --host 0.0.0.0

# ============================================
# 2. Сборка для production
# ============================================
.PHONY: build
build: clean
	@echo "🏗️  Собираем проduction-бандл..."
	@cd frontend && vite build
	@echo "✅ Готово! Результат в папке frontend/dist"

# ============================================
# 3. Просмотр сборки локально
# ============================================
.PHONY: preview
preview:
	@echo "👀 Предпросмотр сборки..."
	@cd frontend && vite preview

# ============================================
# 4. Проверка кода (ESLint / Prettier)
# ============================================
# Если у тебя нет конфига, просто замени на "echo 'Lint не настроен'"
.PHONY: lint
lint:
	@echo "🔎 Запускаем ESLint..."
	@cd frontend && npx eslint $(SRC_DIR) --ext .js,.vue --quiet || true

# ============================================
# 5. Очистка
# ============================================
.PHONY: clean
clean:
	@echo "🧹 Очищаем папку dist..."
	@rm -rf frontend/dist
	@echo "✅ Очистка завершена."

# ============================================
# 6. Дополнительно: Синхронизация с бэком (как в README)
# ============================================
.PHONY: api-register
api-register:
	@echo "📝 Пример регистрации (из README.md)..."
	@echo 'curl -X POST http://localhost:8081/auth/register  -H "Content-Type: application/json"  -d \'{"email": "user@example.com", "password": "mypassword123"}\''

.PHONY: api-login
api-login:
	@echo "📝 Пример логина (из README.md)..."
	@echo 'curl -X POST http://localhost:8081/auth/login -H "Content-Type: application/json" -d \'{"email": "user@example.com", "password": "mypassword123"}\''
