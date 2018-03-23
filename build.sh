#!/bin/bash
set -e
docker-compose build

if [ -n "$REGISTRY" ]; then
    if [ -n "$REGISTRY_USER" ]; then
        docker login -u ${REGISTRY_USER} -p ${REGISTRY_PASS}
    fi
    docker-compose push
fi