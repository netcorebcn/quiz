# docker rm -f quiz-voting-api
# docker rm -f eventstore-node

# docker run --name eventstore-node -d -p 2113:2113 -p 1113:1113 eventstore/eventstore
# docker run --name quiz-voting-api -d -p 5000:80 quiz-voting


./build.sh
docker rm -f $(docker ps -qa)
docker-compose -f ./docker/local-swarm/docker-compose.yml up