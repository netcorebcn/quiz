#!/bin/bash

REPO=$1
SHA=${2:-master}
TOKEN=$3

echo $REPO
echo $SHA

# checkout sha commit from github repo
docker build -t quiz-$SHA-ci https://github.com/$REPO#$SHA -f ./docker/ci/Dockerfile.ci
docker create --name quiz-$SHA-build quiz-$SHA-ci echo > null
docker cp quiz-$SHA-build:/quizapp ./build-$SHA

# build docker images from the git checkout
pushd build-$SHA
./build.sh $SHA

# deploy stack to swarm using docker compose
if [ ! -z "$TOKEN" ]; then
    pushd docker/swarm
    ./deploy.sh $TOKEN $SHA;
    popd
fi

popd

rm -rf build-$SHA
