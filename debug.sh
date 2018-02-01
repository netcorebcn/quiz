docker build -f docker/aspnetcore-debug/Dockerfile -t aspnetcore-debug .

docker-compose stop

docker-compose \
    -f docker-compose.infra.yml \
    -f docker-compose.override.yml \
    -f docker-compose.debug.yml \
    up -d --force-recreate --remove-orphans