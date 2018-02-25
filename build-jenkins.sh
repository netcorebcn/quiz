#!/bin/bash
set -e
docker-compose -f ./jenkins/docker-compose.yml build

if [ -n "$REGISTRY" ]; then

    if [ -n "$REGISTRY_USER" ]; then
        docker login -u ${REGISTRY_USER} -p ${REGISTRY_PASS}
    fi

    docker-compose -f ./jenkins/docker-compose.yml push
fi

for env in REGISTRY TAG REGISTRY_USER GITHUB_REPO JENKINS_URL GITHUB_ADMINS
do
    pattern+='s/${'$env'}/'${!env}'/g;'
done

kubectl config use-context ci && \
sed $pattern ./jenkins/deploy.yml | kubectl apply -f - 