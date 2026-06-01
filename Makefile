build-auth:
	docker build -t auth-service ./backend/auth

run-auth:
	docker run --rm -p 8081:8081 auth-service

build-frontend:
	docker build -t frontend-service ./frontend

run-frontend:
    docker run --rm -p 5173:5173 frontend-service

run: # Сервис на localhost:3000
	docker-compose up --build

install:
	@echo Download Docker - https://www.docker.com/get-started/

install-wsl: # установка подсистемы linux для windows
    wsl --install \
    wsl --install -d Ubuntu

# при ошибках с миграцией
migrate:
	dotnet tool install --global dotnet-ef
	dotnet remove package Microsoft.AspNetCore.OpenApi
	dotnet add package Microsoft.AspNetCore.OpenApi
	dotnet clean
	dotnet restore
	dotnet build
	dotnet ef database update
