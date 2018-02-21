rm -rf secrets
mkdir secrets

pushd secrets
echo -n ${DB_CONNECTION} > dbconnection
echo -n ${MESSAGE_BROKER} > messagebroker
echo -n ${DOCKER_PASS} > docker-pass
echo -n ${DOCKER_USER} > docker-user
echo -n ${GITHUB_TOKEN} > github-token
echo -n ${JENKINS_PASS} > jenkins-pass
echo -n ${JENKINS_USER} > jenkins-user
echo -n ${DB_PASS} > db-pass
echo -n ${DB_USER} > db-user

kubectl delete secret quiz-secrets
kubectl create secret generic quiz-secrets \
          --from-file=dbconnection \
          --from-file=messagebroker \
          --from-file=docker-pass \
          --from-file=docker-user \
          --from-file=github-token \
          --from-file=jenkins-pass \
          --from-file=jenkins-user \
          --from-file=db-pass \
          --from-file=db-user
popd