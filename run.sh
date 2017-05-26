#!/bin/bash
set -e
docker-compose -f docker-compose.yml -f docker-compose.ci.yml up --force-recreate --build