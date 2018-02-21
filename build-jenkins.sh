#!/bin/bash
set -e
if [ -n "$REGISTRY" ]; then
    docker login -u ${DOCKER_USER} -p ${DOCKER_PASSWORD}
else
    export REGISTRY='localhost:5000'
    [ ! "$(docker ps -a | grep registry)" ] && docker run -d -p 5000:5000 --restart=always --name registry registry:2
fi
docker-compose -f ./jenkins/docker-compose.yml build
docker-compose -f ./jenkins/docker-compose.yml push
sed 's/${REGISTRY}/'$REGISTRY'/g;s/${TAG}/'$TAG'/g;s/${DOCKER_USER}/'$DOCKER_USER'/g' ./jenkins/deploy.yml | kubectl apply -f -