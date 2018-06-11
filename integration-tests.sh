#!/bin/bash
set -e
chart='quiz'
chartPath='./deploy/'$chart
deploy=$chart-${BRANCH} 

helm init --client-only

# cleanup previous execution
helm delete $deploy --purge > /dev/null 2>&1 || true

helm dep update ./deploy/$chart
helm upgrade --install \
    $deploy \
    $chartPath \
    --namespace $deploy \
    --set imageRegistry=${REGISTRY} \
    --set imageTag=${TAG} \
    --set postgresql.postgresPassword=${POSTGRES_PASSWORD} \
    --set rabbitmq.rabbitmq.password=${RABBIT_PASSWORD} \
    --set rabbitmq.ingress.host=$deploy-rabbit.quiz.io \
    --debug \
    --wait

helm test --cleanup $deploy

# cleanup
helm delete $deploy --purge
kubectl delete ns $deploy