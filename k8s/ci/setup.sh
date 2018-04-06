#!/bin/bash
namespace=ci
deploy=registry

kubectl apply -f ci.yml
kubectl apply -f $deploy.yml --namespace=$namespace
kubectl rollout status deployment/$deploy -n $namespace