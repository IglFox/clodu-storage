FROM node:20-alpine AS build-stage

WORKDIR /app

COPY . .

RUN npm install
CMD ["npm", "run", "dev"]
