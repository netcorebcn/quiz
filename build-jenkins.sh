#!/bin/bash
set -e
docker-compose -f ./jenkins/docker-compose.yml build

if [ -n "$REGISTRY" ]; then
    docker login -u ${DOCKER_USER} -p ${DOCKER_PASSWORD}
    docker-compose -f ./jenkins/docker-compose.yml push
fi

for env in REGISTRY TAG DOCKER_USER GITHUB_REPO JENKINS_URL GITHUB_ADMINS
do
    pattern+='s/${'$env'}/'${!env}'/g;'
done

sed $pattern ./jenkins/deploy.yml | kubectl apply -f - 