FROM node:alpine
ARG api

WORKDIR ${api}
COPY ${api}package.json .
RUN npm install
COPY ${api} .

ENTRYPOINT ["npm", "run", "start"]