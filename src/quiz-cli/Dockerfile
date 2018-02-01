FROM node:alpine

WORKDIR /app

COPY src/quiz-cli/package.json .
RUN npm install
COPY src/quiz-cli .

ENTRYPOINT ["npm", "run", "start"]