#!/bin/bash
set -e
chart='quiz'
deploy=$chart-${BRANCH} 

helm init --client-only
helm delete $deploy --purge > /dev/null 2>&1 || true

helm dep update ./deploy/$chart
helm upgrade --install \
    $deploy \
    --namespace $deploy \
    --set imageRegistry=${REGISTRY} \
    --set imageTag=${TAG} \
    --set postgresql.postgresPassword= ${POSTGRES_PASSWORD} \
    --set rabbitmq.password= ${RABBIT_PASSWORD} \
    --set rabbitmq.ingress.host = $deploy-rabbit.quiz.io \
    ./deploy/$chart \
    --debug \
    --wait

helm test $deploy --verbose
helm delete $deploy --purge