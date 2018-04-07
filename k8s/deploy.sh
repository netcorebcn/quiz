#!/bin/bash
set -e

namespace=$1
deployments=$2
path=${3:-$namespace}
environment=${4:-'REGISTRY TAG ENVIRONMENT REGISTRY_USER GITHUB_REPO JENKINS_URL GITHUB_ADMINS'}

for env in $environment
do
    pattern+='s,${'$env'},'${!env}',g;'
done

for deploy in $deployments
do
    sed $pattern $path/$deploy.yml | kubectl apply -f - --namespace=$namespace
    kubectl rollout status deployment/$deploy -n $namespace
done