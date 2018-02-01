docker-compose -f docker-compose.infra.yml -f docker-compose.yml stop
docker-compose -f docker-compose.infra.yml -f docker-compose.yml rm -f

docker-compose \
    -f docker-compose.infra.yml \
    -f docker-compose.override.yml \
    -f docker-compose.yml \
    up --build -d --remove-orphans