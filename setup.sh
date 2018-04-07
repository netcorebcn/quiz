pushd k8s
./setup.sh
popd

pushd jenkins
../build.sh
popd

pushd k8s
./deploy.sh ci jenkins-ci
popd
