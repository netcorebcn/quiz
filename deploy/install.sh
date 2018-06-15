#!/bin/bash
set -e
chart='quiz'
deploy=$chart-${QUIZ_ENVIRONMENT:-'pro'}
ingressPrefix=$([ "${QUIZ_ENVIRONMENT}" == 'pro' ] && echo "" || echo "${QUIZ_ENVIRONMENT}.")

echo 'Installing deploy '$deploy' with ingressPrefix '$ingressPrefix
helm init --client-only
helm dep update $chart
helm upgrade --install \
    $deploy \
    $chart \
    --namespace $deploy \
    --set imageRegistry=${REGISTRY} \
    --set imageTag=${TAG} \
    --set ingressHost=${ingressPrefix}${INGRESS_DOMAIN} \
    --set postgresql.postgresPassword=${POSTGRES_PASSWORD} \
    --set rabbitmq.rabbitmq.password=${RABBIT_PASSWORD} \
    --set rabbitmq.ingress.hostName=$ingressPrefix'rabbit.'${INGRESS_DOMAIN} \
    --debug \
    --wait