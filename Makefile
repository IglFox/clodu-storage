build-auth:
	docker build -t auth-service ./backend/auth

run-auth:
	docker run --rm -p 8081:8081 auth-service


build-frontend:
	docker build -t frontend-service ./frontend

run-frontend:
	docker run --rm -p 3000:3000 frontend-service

run: # Сервис на localhost:3000
	docker-compose up --build

install:
	@echo Download Docker - https://www.docker.com/get-started/
