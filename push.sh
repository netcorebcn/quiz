#!/bin/bash
set -e
if [ -n "$REGISTRY" ]; then
    if [ -n "$REGISTRY_USER" ]; then
        docker login -u ${REGISTRY_USER} -p ${REGISTRY_PASS}
    fi
    docker-compose -f docker-compose.yml push
fi