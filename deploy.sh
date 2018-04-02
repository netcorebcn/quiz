#!/bin/bash

if [ -n "$ENVIRONMENT" ]; then
    namespace=${ENVIRONMENT}
else
    namespace='production'
fi

for env in ENVIRONMENT REGISTRY TAG 
do
    pattern+='s/${'$env'}/'${!env}'/g;'
done

for deploy in commands.yml queries.yml ui.yml infra.yml
do
    sed $pattern k8s/quizapp/$deploy | kubectl apply -f - --namespace=$namespace
done
