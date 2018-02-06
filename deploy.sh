#!/bin/bash
set -e
sed 's/${REGISTRY}/'$REGISTRY'/g;s/${TAG}/'$TAG'/g' ./k8s/deploy.yml | kubectl apply -f -
