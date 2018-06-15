#!/bin/bash
set -e
chart='quiz'
deploy=$chart-${TAG_BRANCH} 
integrationPod=$deploy-integration-tests

helm init --client-only
# cleanup previous execution
helm delete $deploy --purge > /dev/null 2>&1 || true
kubectl delete pod $integrationPod -n $deploy > /dev/null 2>&1 || true

helm dep update $chart
helm upgrade --install \
    $deploy \
    $chart \
    --namespace $deploy \
    --set imageRegistry=${REGISTRY} \
    --set imageTag=${TAG} \
    --set ingressHost=${TAG_BRANCH}.${INGRESS_DOMAIN} \
    --set postgresql.postgresPassword=${POSTGRES_PASSWORD} \
    --set postgresql.persistence.enabled=false \
    --set rabbitmq.rabbitmq.password=${RABBIT_PASSWORD} \
    --set rabbitmq.ingress.hostName=${TAG_BRANCH}.rabbit.${INGRESS_DOMAIN} \
    --debug \
    --wait

helm test $deploy

# cleanup
helm delete $deploy --purge
kubectl delete ns $deploy