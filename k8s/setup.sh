#!/bin/bash

# Create Namespaces
if [ -z "$CLUSTER" ]; then
    export CLUSTER=minikube
    export CLUSTER_USER=minikube
fi

for ns in ./namespaces/*.yml
do
    filename="${ns##*/}"
    filename="${filename%.*}"
    kubectl apply -f $ns
    kubectl config set-context $filename --namespace=$filename --cluster=${CLUSTER} --user=${CLUSTER_USER}
done

# Create Secrets
rm -rf secrets
mkdir secrets

pushd secrets
echo -n ${REGISTRY_PASS} > registry-pass
echo -n ${REGISTRY_USER} > registry-user
echo -n ${GITHUB_TOKEN} > github-token
echo -n ${JENKINS_PASS} > jenkins-pass
echo -n ${JENKINS_USER} > jenkins-user

kubectl config use-context ci 
kubectl delete secret quiz-secrets 
kubectl create secret generic quiz-secrets \
          --from-file=registry-pass \
          --from-file=registry-user \
          --from-file=github-token \
          --from-file=jenkins-pass \
          --from-file=jenkins-user

echo -n ${DB_CONNECTION} > dbconnection
echo -n ${MESSAGE_BROKER} > messagebroker
echo -n ${DB_PASS} > db-pass
echo -n ${DB_USER} > db-user

for env in staging production
do
    kubectl config use-context $env 
    kubectl delete secret quiz-secrets 
    kubectl create secret generic quiz-secrets \
            --from-file=dbconnection \
            --from-file=messagebroker \
            --from-file=db-pass \
            --from-file=db-user
done
popd

# Deploy Registry
kubectl config use-context ci
kubectl apply -f ./ci/registry.yml