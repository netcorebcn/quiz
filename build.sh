#!/bin/bash

#clean up
rm -rf build
mkdir build

#run unit tests 
docker build -t quiz-tests-ci -f ./docker/voting/Dockerfile.tests . || { echo "unit test failed"; exit 1; }

for container in voting results setup
do
    #clean up
    docker rm -f quiz-"$container"-build

    #build ci image
    docker build -t quiz-"$container"-ci -f ./docker/"$container"/Dockerfile.build .

    #publish build
    docker create --name quiz-"$container"-build quiz-"$container"-ci
    docker cp quiz-"$container"-build:/out build/"$container"

    #build runtime image
    docker build -t quiz-"$container" -f ./docker/"$container"/Dockerfile .
done

#clean up
docker system prune --force
