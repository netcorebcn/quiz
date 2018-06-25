#!/bin/bash

# brew cask install minikube

# MINIKUBE SETUP
minikube delete
minikube start --memory=8192 --cpus=4 --vm-driver=hyperkit
minikube addons enable ingress 
minikube addons enable heapster
sleep 60

# HELM SETUP
helm init --wait
