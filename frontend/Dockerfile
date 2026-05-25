# Этап сборки
FROM node:22-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# Этап запуска
FROM nginx:alpine
# Удаляем стандартный конфиг Nginx
RUN rm /etc/nginx/conf.d/default.conf
# Копируем наш конфиг
COPY nginx.conf /etc/nginx/conf.d/default.conf
# Копируем собранный фронтенд в папку Nginx
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
