#!/bin/bash
set -e
chart='quiz'
deploy=$chart-${QUIZ_ENVIRONMENT:-'pro'}
ingressPrefix=$([ "${QUIZ_ENVIRONMENT}" == 'pro' ] && echo "" || echo "${QUIZ_ENVIRONMENT}.")
integrationPod=$deploy-integration-tests

# cleanup previous integration test
kubectl delete pod $integrationPod -n $deploy > /dev/null 2>&1 || true

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
    
# run integration tests
helm test $deploy
kubectl delete pod $integrationPod -n $deploy 