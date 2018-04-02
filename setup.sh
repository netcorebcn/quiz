pushd k8s/ci
./setup.sh
popd

pushd jenkins
./build.sh
popd

pushd k8s/ci
./deploy.sh
popd

pushd k8s/quizapp
./setup.sh
popd