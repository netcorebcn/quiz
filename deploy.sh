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

for deploy in db messagebroker quiz-commands quiz-queries quiz-ui
do
    sed $pattern k8s/quizapp/$deploy.yml | kubectl apply -f - --namespace=$namespace
    kubectl rollout status deployment/$deploy -n $namespace
done