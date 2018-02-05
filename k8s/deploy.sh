#!/bin/bash
sed 's/${REGISTRY}/'$REGISTRY'/g;s/${TAG}/'$TAG'/g' deploy.yml | kubectl apply -f -