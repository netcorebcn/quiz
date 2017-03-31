FROM node:onbuild

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

RUN chmod +x ./run.sh
EXPOSE 8080