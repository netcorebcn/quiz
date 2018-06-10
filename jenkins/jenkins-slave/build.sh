##!/bin/bash
set -e
docker-compose build

if [ -n "$REGISTRY" ]; then
    docker-compose push
fi