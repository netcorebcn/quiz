#!/bin/bash
set -e

for deploy in k8s/*.yml
do
    sed 's/${REGISTRY}/'$REGISTRY'/g;s/${TAG}/'$TAG'/g' $deploy | kubectl apply -f -
done
