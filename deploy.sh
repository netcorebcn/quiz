#!/bin/bash
set -e

for env in ENVIRONMENT REGISTRY TAG 
do
    pattern+='s/${'$env'}/'${!env}'/g;'
done

if [ -n "$ENVIRONMENT" ]; then
    kubectl config use-context ${ENVIRONMENT}  
else
    kubectl config use-context production
fi

for deploy in ./k8s/services/*.yml
do
    sed $pattern $deploy | kubectl apply -f - 
done