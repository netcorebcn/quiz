# builder image
FROM node AS builder

WORKDIR .

ARG NODE_ENV
ENV NODE_ENV $NODE_ENV
COPY src/quiz-ui/package.json .
RUN npm install
COPY src/quiz-ui .
RUN npm run build

# build production image
FROM node:alpine
RUN npm install -g serve

WORKDIR /app
COPY --from=builder build/ .
ENTRYPOINT ["serve", "-s ./"]