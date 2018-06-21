#!/bin/bash
set -e
chart='quiz-ci'

helm dep update $chart
helm upgrade --install \
    $chart \
    --namespace $chart \
    --set global.jenkinsHostName=${JENKINS_HOST} \
    --set global.github.admin=${GITHUB_USER} \
    --set global.github.token=${GITHUB_TOKEN} \
    --set registryHostName=registry.${INGRESS_DOMAIN} \
    --set jenkins.Master.HostName=jenkins.${INGRESS_DOMAIN} \
    --set jenkins.Master.AdminPassword=${JENKINS_PASSWORD} \
    --set jenkins.Agent.Image=${REGISTRY}/jenkins-slave \
    --set jenkins.Agent.ImageTag=${TAG} \
    $chart \
    --wait

pushd jenkins-slave
./build.sh
popd