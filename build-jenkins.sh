#!/bin/bash
set -e
docker-compose -f ./jenkins/docker-compose.yml build

if [ -n "$REGISTRY" ]; then
    docker login -u ${DOCKER_USER} -p ${DOCKER_PASSWORD}
    docker-compose -f ./jenkins/docker-compose.yml push
fi

sed 's/${REGISTRY}/'$REGISTRY'/g;s/${TAG}/'$TAG'/g' ./jenkins/deploy.yml | kubectl apply -f -