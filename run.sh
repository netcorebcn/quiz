./build.sh
docker rm -f $(docker ps -qa)
docker-compose -f ./docker/docker-compose.eventstore.yml up -d
sleep 5
docker-compose -f ./docker/docker-compose.eventstore.setup.yml up -d
sleep 5
docker-compose -f ./docker/docker-compose.yml up