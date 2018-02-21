rm -rf secrets
mkdir secrets

pushd secrets
echo -n ${DB_CONNECTION} > dbconnection
echo -n ${MESSAGE_BROKER} > messagebroker
echo -n ${REGISTRY_PASS} > registry-pass
echo -n ${REGISTRY_USER} > registry-user
echo -n ${GITHUB_TOKEN} > github-token
echo -n ${JENKINS_PASS} > jenkins-pass
echo -n ${JENKINS_USER} > jenkins-user
echo -n ${DB_PASS} > db-pass
echo -n ${DB_USER} > db-user

kubectl delete secret quiz-secrets
kubectl create secret generic quiz-secrets \
          --from-file=dbconnection \
          --from-file=messagebroker \
          --from-file=registry-pass \
          --from-file=registry-user \
          --from-file=github-token \
          --from-file=jenkins-pass \
          --from-file=jenkins-user \
          --from-file=db-pass \
          --from-file=db-user
popd