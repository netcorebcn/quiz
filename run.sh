#!/bin/bash
set -e
docker rm -f $(docker ps -qa)
docker-compose build
docker-compose up