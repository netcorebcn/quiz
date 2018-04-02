#!/bin/bash
kubectl delete pod integration-tests

set -e
kubectl run --attach integration-tests \
--image=${REGISTRY}/quiz-cli:${TAG} \
--restart=Never \
--env="QUIZ_URL=quiz-commands" \
--env="ITERATIONS=5" 

set +e
kubectl delete pod integration-tests