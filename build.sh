#!/bin/bash
sha=${1:-latest}

#clean up
rm -rf build
mkdir build

#run unit tests 
docker build -t quiz-tests-ci -f ./docker/voting/Dockerfile.tests . || { echo "unit test failed"; exit 1; }

for container in voting results setup
do
    #build ci image
    docker build -t quiz-$container-ci:$sha -f ./docker/$container/Dockerfile.build .

    #publish build
    docker create --name quiz-$container-build quiz-$container-ci:$sha
    docker cp quiz-$container-build:/out build/$container

    #build runtime image
    docker build -t quiz-$container:$sha -f ./docker/$container/Dockerfile .
done

#build ci
docker build -t awesome-ci:$sha -f ./docker/ci/Dockerfile .

#clean up
docker system prune --force
