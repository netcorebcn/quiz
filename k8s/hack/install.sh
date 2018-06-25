#!/bin/bash
set -e
eval $(minikube docker-env)

# install ci cd
pushd ../jenkins
./install.sh
popd