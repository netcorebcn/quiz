#!/bin/bash
registry=$1
sha=${2:-latest}

echo Registry:$registry
echo Sha:$sha

#clean up
rm -rf build
mkdir build

#run unit tests 
docker build -t quiz-tests-ci:$sha -f ./docker/voting/Dockerfile.tests . || { echo "unit test failed"; exit 1; }

for container in voting results setup
do
    #build ci image
    docker build -t quiz-$container-ci:$sha -f ./docker/$container/Dockerfile.build .

    #publish build
    docker create --name quiz-$container-build quiz-$container-ci:$sha
    docker cp quiz-$container-build:/out build/$container

    #build runtime image
    docker build -t $registry"quiz-"$container:$sha -f ./docker/$container/Dockerfile .
 
    #push to registry
    if [ ! -z "$registry" ]; then 
        docker push $registry"quiz-"$container:$sha 
    fi
done

#build ci
docker build -t $registry"awesome-ci:"$sha -f ./docker/ci/Dockerfile .
if [ ! -z "$registry" ]; then 
    docker push $registry"awesome-ci:"$sha 
fi

#clean up
docker system prune --force

