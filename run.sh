./build.sh
docker rm -f $(docker ps -qa)
docker-compose -f ./docker/docker-compose.eventstore.yml up -d
sleep 10
docker-compose -f ./docker/docker-compose.yml up