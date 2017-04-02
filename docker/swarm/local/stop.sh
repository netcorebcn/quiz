docker swarm leave --force
docker rm -f $(docker ps -qa)
