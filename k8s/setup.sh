#!/bin/bash

# Setup ci
namespace=ci

kubectl apply -f $namespace/$namespace.yml
kubectl delete secret quiz-secrets --namespace=$namespace
kubectl create secret generic quiz-secrets \
          --from-literal=registry-pass=${REGISTRY_PASS} \
          --from-literal=registry-user=${REGISTRY_USER} \
          --from-literal=github-token=${GITHUB_TOKEN} \
          --from-literal=jenkins-pass=${JENKINS_PASS} \
          --from-literal=jenkins-user=${JENKINS_USER} \
          --namespace=$namespace

# Setup staging and production
for env in staging production
do
    kubectl apply -f quizapp/$env.yml
    kubectl delete secret quiz-secrets --namespace=$env
    kubectl create secret generic quiz-secrets \
            --from-literal=dbconnection=${DB_CONNECTION} \
            --from-literal=messagebroker=${MESSAGE_BROKER} \
            --from-literal=db-pass=${DB_PASS} \
            --from-literal=db-user=${DB_USER} \
            --namespace=$env
done
