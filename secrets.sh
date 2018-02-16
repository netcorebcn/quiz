rm -rf secrets
mkdir secrets

pushd secrets
echo ${DB_CONNECTION} > dbconnection
echo ${MESSAGE_BROKER} > messagebroker
echo ${DOCKER_PASS} > docker-pass
echo ${JENKINS_PASS} > jenkins-pass
echo ${JENKINS_USER} > jenkins-user
echo ${DB_PASS} > db-pass
echo ${DB_USER} > db-user

kubectl delete secret quiz-secrets
kubectl create secret generic quiz-secrets \
          --from-file=dbconnection \
          --from-file=messagebroker \
          --from-file=docker-pass \
          --from-file=jenkins-pass \
          --from-file=jenkins-user \
          --from-file=db-pass \
          --from-file=db-user
popd