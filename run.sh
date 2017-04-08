./build.sh
docker rm -f $(docker ps -qa)
docker-compose -f ./docker/docker-compose.yml up