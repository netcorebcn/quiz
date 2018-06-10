#!/bin/bash

# export as env all the needed secrets
export $(cat secrets)
# connect to minikube docker engine
eval $(minikube docker-env)

# deploy heapster monitoring and nginx ingress as addons
minikube addons enable ingress 
minikube addons enable heapster

# install tiller
helm init --wait

# install local registry and jenkins chart
pushd jenkins
./install.sh
popd

# build and install quiz app
./build.sh
./deploy.sh