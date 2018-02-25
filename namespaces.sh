#!/bin/bash

if [ -z "$CLUSTER" ]; then
    export CLUSTER=minikube
    export CLUSTER_USER=minikube
fi

for ns in k8s/namespaces/*.yml
do
    filename="${ns##*/}"
    filename="${filename%.*}"
    kubectl apply -f $ns
    kubectl config set-context $filename --namespace=$filename --cluster=${CLUSTER} --user=${CLUSTER_USER}
done