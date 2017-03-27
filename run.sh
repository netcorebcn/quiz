./build.sh
docker rm -f $(docker ps -qa)
docker-compose -f ./docker/swarm/docker-compose.yml up