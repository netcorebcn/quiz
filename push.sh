#!/bin/bash
set -e
if [ -n "$REGISTRY" ]; then
    docker login -u ${DOCKER_USER} -p ${DOCKER_PASSWORD}
    docker-compose -f docker-compose.yml push
fi