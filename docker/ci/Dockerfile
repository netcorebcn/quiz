FROM node

#install docker client
RUN apt-get update && \
    apt-get install \
        apt-transport-https \
        ca-certificates \
        curl \
        software-properties-common -y && \
    curl -fsSL https://download.docker.com/linux/debian/gpg | apt-key add - && \
    add-apt-repository \
        "deb [arch=amd64] https://download.docker.com/linux/debian \
        $(lsb_release -cs) \
        stable" && \ 
    apt-get update && \
    apt-get install docker-ce -y

RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

ARG NODE_ENV
ENV NODE_ENV $NODE_ENV
COPY src/awesome-ci/package.json /usr/src/app/
RUN npm install && npm cache clean
COPY src/awesome-ci /usr/src/app
RUN chmod +x ./run.sh

CMD [ "npm", "start" ]

EXPOSE 8080