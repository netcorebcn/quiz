pushd k8s
./setup.sh
popd

k8s
./deploy.sh ci registry
popd

pushd jenkins
../build.sh
popd

pushd k8s
./deploy.sh ci jenkins-ci
popd
