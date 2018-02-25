#!/bin/bash
set -e

kubectl run integration-tests \
--image=${REGISTRY}/quiz-cli:${TAG} \
--env="QUIZ_URL=quiz-commands"