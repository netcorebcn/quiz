CI_TOKEN=$1
REPO=$2
REGISTRY=$3

docker service create --name registry --publish 5000:5000 registry:2
docker service create \
  --publish=9090:8080 \
  --limit-cpu 0.5 \
  --name=viz \
  --env PORT=9090 \
  --constraint=node.role==manager \
  --mount=type=bind,src=/var/run/docker.sock,dst=/var/run/docker.sock \
  manomarks/visualizer

#run CI/CD on master branch
pushd ../../src/awesome-ci
    ./run.sh \
        $REPO \
        master \
        $REGISTRY \
        $CI_TOKEN
popd
