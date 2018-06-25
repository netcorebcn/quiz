#!/bin/bash
set -e
chart='quiz'

helm init --client-only
helm dep update $chart

cleanUp() {
    deploy=$1
    helm delete $deploy --purge \
    && kubectl delete ns $deploy > /dev/null 2>&1 || true
    sleep 30
}

runTests() {
    deploy=$1
    kubectl delete pod $deploy-integration-tests -n $deploy > /dev/null 2>&1 || true 
    sleep 30
    helm test $deploy
}

getEnvironment() {
    echo ${QUIZ_ENVIRONMENT:-'production'}
}

getBranch() {
    echo ${TAG:-'master'}
}

getDomain() {
    environment=${1:-$(getEnvironment)}
    domain=${INGRESS_DOMAIN} 
    [ "$environment" == 'production' ] && echo $domain || echo $environment.$domain
}
