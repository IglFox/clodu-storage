# Стадия сборки
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY ["backend/Clodu.API/Clodu.API.csproj", "Clodu.API/"]
RUN dotnet restore "Clodu.API/Clodu.API.csproj"

# Копируем весь код и собираем
COPY . .
WORKDIR "/src/backend/Clodu.API"
RUN dotnet build "Clodu.API.csproj" -c Release -o /app/build

# Публикация
FROM build AS publish
RUN dotnet publish "Clodu.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Clodu.API.dll"]
