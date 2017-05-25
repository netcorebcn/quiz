#!/bin/bash
set -e
BUILD_CONTEXT=$1
TAG=$2
REGISTRY=$3

echo BUILD_CONTEXT:$BUILD_CONTEXT
echo TAG:$TAG
echo REGISTRY:$REGISTRY

if [ ! -z $REGISTRY ]; then export REGISTRY=$REGISTRY; fi
if [ ! -z $TAG ]; then export TAG=$TAG; fi
if [ ! -z $BUILD_CONTEXT ]; then export BUILD_CONTEXT=$BUILD_CONTEXT; fi

docker-compose kill \
&& docker-compose build
if [ ! -z $REGISTRY ]; then docker-compose push; fi
