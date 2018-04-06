#!/bin/bash
set -e

# Create Secrets
rm -rf secrets
mkdir secrets

pushd secrets
echo -n ${REGISTRY_PASS} > registry-pass
echo -n ${REGISTRY_USER} > registry-user
echo -n ${GITHUB_TOKEN} > github-token
echo -n ${JENKINS_PASS} > jenkins-pass
echo -n ${JENKINS_USER} > jenkins-user

kubectl delete secret quiz-secrets --namespace=ci
kubectl create secret generic quiz-secrets \
          --from-file=registry-pass \
          --from-file=registry-user \
          --from-file=github-token \
          --from-file=jenkins-pass \
          --from-file=jenkins-user \
          --namespace=ci
popd
rm -rf secrets

for env in REGISTRY TAG REGISTRY_USER GITHUB_REPO JENKINS_URL GITHUB_ADMINS
do
    pattern+='s,${'$env'},'${!env}',g;'
done

sed $pattern jenkins-ci.yml | kubectl apply -f - --namespace=ci
kubectl rollout status deployment/jenkins-ci -n ci