#!/bin/bash
set -e

for deploy in k8s/*.yml
do
    for env in ENVIRONMENT REGISTRY TAG 
    do
        pattern+='s/${'$env'}/'${!env}'/g;'
    done

    sed $pattern ./jenkins/deploy.yml | kubectl apply -f - 
done
