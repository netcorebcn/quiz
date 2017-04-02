#!/bin/bash

REPO=$1
SHA=${2:-master}
REGISTRY=$3
TOKEN=$4

echo Repo:$REPO
echo Sha:$SHA
echo Token:$TOKEN
echo Registry:$REGISTRY

# checkout sha commit from github repo
docker build -t quiz-$SHA-ci https://github.com/$REPO#$SHA -f ./docker/ci/Dockerfile.ci
docker create --name quiz-$SHA-build quiz-$SHA-ci echo ""
docker cp quiz-$SHA-build:/quizapp ./build-$SHA

# build docker images from the git checkout
pushd build-$SHA
./build.sh $REGISTRY $SHA

# deploy stack to swarm using docker compose
if [ ! -z "$TOKEN" ]; then
    pushd docker/swarm
    ./deploy.sh $REGISTRY $TOKEN $SHA;
    popd
fi

popd
# clean up
docker rmi -f quiz-$SHA-ci
rm -rf build-$SHA