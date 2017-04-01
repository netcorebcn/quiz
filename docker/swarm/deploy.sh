#!/bin/bash
token=$1
sha=${2:-latest}

export CI_TOKEN=$token
export SHA_COMMIT=$sha

echo $CI_TOKEN
echo $SHA_COMMIT

docker deploy --compose-file docker-compose.yml stack
docker service ls