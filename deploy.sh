#!/bin/bash
sed 's/${REGISTRY}/'$REGISTRY'/g;s/${TAG}/'$TAG'/g' ./k8s/deploy.yml | kubectl apply -f -