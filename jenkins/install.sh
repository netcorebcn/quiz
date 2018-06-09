#!/bin/bash
chart='quiz-ci'

helm dep update ./$chart
helm upgrade --install \
    $chart \
    --namespace $chart \
    --set global.github.admin=${GITHUB_ADMIN} \
    --set global.github.token=${GITHUB_TOKEN} \
    --set jenkins.Master.AdminPassword=${JENKINS_PASSWORD} \
    --set jenkins.Agent.Image=${REGISTRY}/jenkins-slave \
    --set jenkins.Agent.ImageTag=${TAG} \
    --set infra.rabbitPassword=${RABBIT_PASSWORD} \
    --set infra.postgresPassword=${POSTGRES_PASSWORD} \
    ./$chart \
    --debug \
    --wait

pushd jenkins-slave
./build.sh
popd