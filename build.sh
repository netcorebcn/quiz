#!/bin/bash
set -e
registry=$1
sha=${2:-latest}

echo Registry:$registry
echo Sha:$sha

#clean up
rm -rf build
mkdir build

#build docker images
for containerPath in ./docker/containers/* ; do
    container=${containerPath##*/}

    #run unit tests 
    if [ -f $containerPath/Dockerfile.tests ]; then 
        docker build -t $container-tests:$sha -f $containerPath/Dockerfile.tests .; 
    fi    

    if [ -f $containerPath/Dockerfile.build ]; then 
        #build ci image
        docker build -t $container-ci:$sha -f $containerPath/Dockerfile.build .
        #publish build
        docker rm --force $(docker ps -qa --filter "name=$container-build-$sha") > /dev/null 2>&1 || true
        docker create --name $container-build-$sha $container-ci:$sha
        docker cp $container-build-$sha:/build build/$container    
    fi

    #build runtime image
    docker build -t $registry$container:$sha -f $containerPath/Dockerfile .
 
    #push runtime image to registry
    if [ ! -z $registry ]; then docker push $registry$container:$sha; fi
done

#clean up
docker system prune --force
