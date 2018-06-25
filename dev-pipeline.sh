#!/bin/bash
set -e
eval $(minikube docker-env)

branch='dev'
commit=$(git log -n 1 --pretty=format:'%h')

export TAG=$branch-$commit
export REGISTRY=localhost:30400
export QUIZ_ENVIRONMENT='prepro'

echo 'Building for '${TAG}
./build.sh

pushd ../deploy
./test.sh
./install.sh
popd
