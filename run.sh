./build.sh
docker rm -f $(docker ps -qa)
export SHA_COMMIT=latest
docker-compose -f ./docker/docker-compose.yml up