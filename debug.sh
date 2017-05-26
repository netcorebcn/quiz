#!/bin/bash
dotnet publish -c Debug -o bin/PublishOutput
docker-compose -f docker-compose.yml -f docker-compose.debug.yml up --force-recreate