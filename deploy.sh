#!/bin/bash
set -e
chart='quiz'
deploy=$chart-production 

helm upgrade --install \
    $deploy \
    --namespace $deploy \
    --set imageRegistry=${REGISTRY} \
    --set imageTag=${TAG} \
    --set postgresql.postgresPassword= ${POSTGRES_PASSWORD} \
    --set rabbitmq.password= ${RABBIT_PASSWORD} \
    --set rabbitmq.ingress.host = 'rabbit.quiz.io' \
    ./deploy/$chart \
    --debug \
    --wait