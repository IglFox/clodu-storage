FROM node:20-alpine AS build-stage

WORKDIR /app

COPY . .

RUN npm install
CMD ["npm", "run", "dev", "--", "--host", "0.0.0.0", "--port", "5173"]
