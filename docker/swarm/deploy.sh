#!/bin/bash
registry=$1
token=$2
sha=${3:-latest}

export REGISTRY=$registry
export CI_TOKEN=$token
export SHA_COMMIT=$sha

echo Registry:$REGISTRY
echo Token:$CI_TOKEN
echo Sha:$SHA_COMMIT

docker deploy --compose-file docker-compose.yml stack
docker service ls