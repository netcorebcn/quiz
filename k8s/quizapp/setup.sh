#!/bin/bash

# Create Namespaces
kubectl apply -f staging.yml
kubectl apply -f production.yml

# Create Secrets
for env in staging production
do
    kubectl delete secret quiz-secrets --namespace=$env
    kubectl create secret generic quiz-secrets \
            --from-literal=dbconnection=${DB_CONNECTION} \
            --from-literal=messagebroker=${MESSAGE_BROKER} \
            --from-literal=db-pass=${DB_PASS} \
            --from-literal=db-user=${DB_USER} \
            --namespace=$env
done
