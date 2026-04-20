build-auth:
	docker build -t auth-service ./backend/auth

run-auth:
	docker run --rm -p 8081:8081 auth-service

run: # Сервис на localhost:3000
	docker-compose up --build

install:
	@echo Download Docker - https://www.docker.com/get-started/
