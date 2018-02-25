#!/bin/bash
set -e

for env in ENVIRONMENT REGISTRY TAG 
do
    pattern+='s/${'$env'}/'${!env}'/g;'
done

for deploy in k8s/*.yml
do
    sed $pattern $deploy | kubectl apply -f - 
done
