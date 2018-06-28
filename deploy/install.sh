#!/bin/bash
set -e
. common.sh

deploy=$chart-$(getEnvironment)
environmentDomain=$(getDomain)

helm upgrade --install \
    $deploy \
    $chart \
    --namespace $deploy \
    --set imageRegistry=${REGISTRY} \
    --set imageTag=${TAG} \
    --set ingressHost=$environmentDomain \
    --set rabbitmq.ingress.hostName='rabbit.'$environmentDomain \
    --set end2end.enabled=true \
    --set integration.enabled=false \
    --debug \
    --wait
    
runTests $deploy
