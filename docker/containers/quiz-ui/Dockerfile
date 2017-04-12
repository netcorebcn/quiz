FROM node
RUN npm install -g serve

WORKDIR /app
COPY build/quiz-ui .
ENTRYPOINT ["serve", "-s ./"]