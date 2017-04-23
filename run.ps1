./build.ps1
docker rm -f $(docker ps -qa)
docker-compose -f ./docker/docker-compose.yml up