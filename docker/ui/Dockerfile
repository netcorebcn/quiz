FROM node
RUN npm install -g serve

WORKDIR /app
COPY build/ui .
ENTRYPOINT ["serve", "-s ./"]