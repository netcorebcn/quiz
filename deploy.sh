#!/bin/bash
set -e
chart='quiz'
chartPath='./deploy/'$chart
deploy=$chart'-production'

helm init --client-only
helm dep update $chartPath
helm upgrade --install \
    $deploy \
    $chartPath \
    --namespace $deploy \
    --set imageRegistry=${REGISTRY} \
    --set imageTag=${TAG} \
    --set postgresql.postgresPassword=${POSTGRES_PASSWORD} \
    --set rabbitmq.rabbitmq.password=${RABBIT_PASSWORD} \
    --set rabbitmq.ingress.host='rabbit.quiz.io' \
    --debug \
    --wait