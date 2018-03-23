#!/bin/bash
set -e

for env in ENVIRONMENT REGISTRY TAG 
do
    pattern+='s/${'$env'}/'${!env}'/g;'
done

kubectl config use-context ${ENVIRONMENT}  

for deploy in ./k8s/services/*.yml
do
    sed $pattern $deploy | kubectl apply -f - 
done