FROM node:latest
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

EXPOSE 5005

ENV LANG=C.UTF-8
ENV LC_ALL=C.UTF-8
ENV TZ=America/Mexico_City

CMD ["node", "grpc-server.js"]