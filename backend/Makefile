.PHONY: help dev-auth dev-auth-debug build-auth clean db-view

# Цвета для вывода
GREEN := \033[0;32m
YELLOW := \033[1;33m
RED := \033[0;31m
NC := \033[0m # No Color

help: ## Показать все доступные команды
	@echo "$(GREEN)Доступные команды:$(NC)"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  $(YELLOW)%-15s$(NC) %s\n", $$1, $$2}'

dev-auth: ## Запустить auth-service (стандартный режим)
	@echo "$(GREEN)🚀 Запускаю auth-service...$(NC)"
	cd backend/auth && cargo run

dev-auth-debug: ## Запустить auth-service с подробными логами
	@echo "$(GREEN)🐛 Запускаю auth-service в режиме DEBUG...$(NC)"
	cd backend/auth && RUST_LOG=debug cargo run

build-auth: ## Собрать auth-service (release)
	@echo "$(GREEN)🔨 Собираю auth-service (release)...$(NC)"
	cd backend/auth && cargo build --release
	@echo "$(GREEN)✅ Бинарник: backend/auth/target/release/auth-service$(NC)"

clean: ## Очистить все артефакты сборки
	@echo "$(RED)🧹 Очищаю артефакты...$(NC)"
	cd backend/auth && cargo clean
	rm -f backend/auth/auth.db
	@echo "$(GREEN)✅ Готово$(NC)"

db-view: ## Показать содержимое базы данных SQLite
	@echo "$(GREEN)📊 Содержимое auth.db:$(NC)"
	@sqlite3 backend/auth/auth.db "SELECT id, email, created_at FROM users;" 2>/dev/null || echo "$(RED)❌ База пуста или не существует$(NC)"

db-reset: ## Сбросить базу данных (удалить auth.db)
	@echo "$(RED)⚠️  Удаляю auth.db...$(NC)"
	rm -f backend/auth/auth.db
	@echo "$(GREEN)✅ База сброшена$(NC)"

test-auth: ## Протестировать API (регистрация + логин + профиль)
	@echo "$(GREEN)🧪 Тестирую auth-service API...$(NC)"
	@echo ""
	@echo "$(YELLOW)1. Регистрация:$(NC)"
	@curl -s -X POST http://localhost:8081/auth/register \
		-H "Content-Type: application/json" \
		-d '{"email": "test@example.com", "password": "secret123"}' | jq . 2>/dev/null || echo "$(RED)❌ Сервер не запущен?$(NC)"
	@echo ""
	@echo "$(YELLOW)2. Логин:$(NC)"
	@TOKEN=$$(curl -s -X POST http://localhost:8081/auth/login \
		-H "Content-Type: application/json" \
		-d '{"email": "test@example.com", "password": "secret123"}' | jq -r .access_token 2>/dev/null); \
	if [ -n "$$TOKEN" ] && [ "$$TOKEN" != "null" ]; then \
		echo "   Токен: $${TOKEN:0:50}..."; \
		echo ""; \
		echo "$(YELLOW)3. Профиль:$(NC)"; \
		curl -s -X GET http://localhost:8081/auth/me \
			-H "Authorization: Bearer $$TOKEN" | jq . 2>/dev/null; \
	else \
		echo "$(RED)❌ Не удалось получить токен$(NC)"; \
	fi

install-tools: ## Установить дополнительные инструменты (sqlite3, jq, curl)
	@echo "$(GREEN)📦 Устанавливаю инструменты...$(NC)"
	sudo apt update
	sudo apt install -y sqlite3 curl jq
	@echo "$(GREEN)✅ Готово$(NC)"