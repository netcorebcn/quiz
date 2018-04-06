#!/bin/bash
namespace=ci
deploy=jenkins-ci

# Create Secrets
kubectl delete secret quiz-secrets --namespace=ci
kubectl create secret generic quiz-secrets \
          --from-literal=registry-pass=${REGISTRY_PASS} \
          --from-literal=registry-user=${REGISTRY_USER} \
          --from-literal=github-token=${GITHUB_TOKEN} \
          --from-literal=jenkins-pass=${JENKINS_PASS} \
          --from-literal=jenkins-user=${JENKINS_USER} \
          --namespace=$namespace

for env in REGISTRY TAG REGISTRY_USER GITHUB_REPO JENKINS_URL GITHUB_ADMINS
do
    pattern+='s,${'$env'},'${!env}',g;'
done


sed $pattern ${deploy}.yml | kubectl apply -f - --namespace=$namespace
kubectl rollout status deployment/$deploy -n $namespace