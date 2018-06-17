#!/bin/bash
set -e

eval $(minikube docker-env)
helm init --client-only

# install ci cd
pushd jenkins
./install.sh
popd

# build quiz docker images
./build.sh

# deploy quiz charts
pushd deploy
./tests.sh
./install.sh
popd