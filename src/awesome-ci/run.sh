#!/bin/bash

REPO=$1
SHA=$2

echo $REPO
echo $SHA

docker build -t quiz-$SHA-ci https://github.com/$REPO#$SHA -f ./docker/ci/Dockerfile.ci
docker create --name quiz-$SHA-build quiz-$SHA-ci echo > null
docker cp quiz-$SHA-build:/quizapp ./build-$SHA

pushd build-$SHA
./build.sh $SHA
popd

rm -rf build/$SHA
