#!/bin/bash
if [ -n "$1" ]; then
    export REGISTRY=$1
fi
if [ -n "$2" ]; then
    export TAG=$2
fi
docker-compose -f docker-compose.yml build