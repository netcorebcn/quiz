#!/bin/bash
set -e
for env in REGISTRY TAG REGISTRY_USER GITHUB_REPO JENKINS_URL GITHUB_ADMINS
do
    pattern+='s,${'$env'},'${!env}',g;'
done

kubectl config use-context ci
sed $pattern jenkins.yml | kubectl apply -f - 
