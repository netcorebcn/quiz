#!/bin/bash

# Create Namespaces
kubectl apply -f staging.yml
kubectl apply -f production.yml

# Create Secrets
rm -rf secrets
mkdir secrets

pushd secrets
echo -n ${DB_CONNECTION} > dbconnection
echo -n ${MESSAGE_BROKER} > messagebroker
echo -n ${DB_PASS} > db-pass
echo -n ${DB_USER} > db-user

for env in staging production
do
    kubectl delete secret quiz-secrets --namespace=$env
    kubectl create secret generic quiz-secrets \
            --from-file=dbconnection \
            --from-file=messagebroker \
            --from-file=db-pass \
            --from-file=db-user \
            --namespace=$env
done
popd
rm -rf secrets
