CI_TOKEN=$1
REPO=$2
REGISTRY=$3

docker deploy --compose-file docker-compose.setup.yml stack

#run CI/CD on master branch
pushd ../../src/awesome-ci
    ./run.sh \
        $REPO \
        master \
        $REGISTRY \
        $CI_TOKEN
popd
