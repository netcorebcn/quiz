#!/bin/bash

REPO=$1
SHA=$2
TOKEN=$3

echo $REPO
echo $SHA

# checkout sha commit from github repo
docker build -t quiz-$SHA-ci https://github.com/$REPO#$SHA -f ./docker/ci/Dockerfile.ci
docker create --name quiz-$SHA-build quiz-$SHA-ci echo > null
docker cp quiz-$SHA-build:/quizapp ./build-$SHA

pushd build-$SHA

./build.sh $SHA

if [ ! -z "$TOKEN" ] then
    ./docker/swarm/deploy.sh $TOKEN $SHA
fi

popd

rm -rf build/$SHA
