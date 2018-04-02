#!/bin/bash

namespace=${ENVIRONMENT}

kubectl delete pod integration-tests --namespace=$namespace

set -e
kubectl run --attach integration-tests \
--image=${REGISTRY}/quiz-cli:${TAG} \
--restart=Never \
--env="QUIZ_URL=quiz-commands" \
--env="ITERATIONS=5" \
--namespace=$namespace

set +e
kubectl delete pod integration-tests --namespace=$namespace