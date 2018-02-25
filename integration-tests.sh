#!/bin/bash
set -e

kubectl delete deployment integration-tests 2>&1

kubectl run --attach integration-tests \
--image=${REGISTRY}/quiz-cli:${TAG} \
--env="QUIZ_URL=quiz-commands"

kubectl delete deployment integration-tests