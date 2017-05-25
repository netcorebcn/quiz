#!/bin/bash
set -e
REPO=$1
SHA=${2:-master}
REGISTRY=$3
TOKEN=$4

echo Repo:$REPO
echo Sha:$SHA
echo Registry:$REGISTRY
echo Token:$TOKEN

# clean up
rm -rf build-$SHA

# checkout sha commit from github repo
docker build -t quiz-$SHA-ci https://github.com/$REPO#$SHA -f ./src/awesome-ci/Dockerfile.ci --no-cache
docker rm --force $(docker ps -qa --filter "name=quiz-$SHA-build") > /dev/null 2>&1 || true
docker create --name quiz-$SHA-build quiz-$SHA-ci echo ""
docker cp quiz-$SHA-build:/quizapp ./build-$SHA

pushd build-$SHA

export TAG=$SHA
export REGISTRY=$REGISTRY
export CI_TOKEN=$TOKEN

docker-compose build
if [ ! -z $REGISTRY ]; then docker-compose push; fi
if [ ! -z $TOKEN ]; then docker deploy --compose-file ./docker/swarm/docker-compose.yml stack; fi

popd

# clean up
docker rmi -f quiz-$SHA-ci
rm -rf build-$SHA