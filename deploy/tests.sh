#!/bin/bash
set -e
. common.sh

branch=$(getBranch)
deploy=$chart-$branch
branchDomain=$(getDomain $branch)

cleanUp $deploy
helm upgrade --install \
    $deploy \
    $chart \
    --namespace $deploy \
    --set imageRegistry=${REGISTRY} \
    --set imageTag=${TAG} \
    --set ingressHost=$branchDomain \
    --set postgresql.persistence.enabled=false \
    --set rabbitmq.ingress.hostName='rabbit.'$branchDomain \
    --set end2end.enabled=false \
    --set integration.enabled=true \
    --debug \
    --wait

runTests $deploy
cleanUp $deploy