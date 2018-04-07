#!/bin/bash
set -e

pushd k8s
./deploy.sh ${ENVIRONMENT} 'db messagebroker quiz-commands quiz-queries quiz-ui' quizapp
popd