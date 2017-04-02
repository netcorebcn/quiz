SERVICE_PRINCIPAL_ID=$1
SERVICE_PRINCIPAL_SECRET=$2
SSH_PUBLIC_KEY=$3
GROUP_NAME=$4
# CI_TOKEN=$5
# REPO=$6

PARAMETERS='{"adServicePrincipalAppID":{"value":"'$SERVICE_PRINCIPAL_ID'"},
"adServicePrincipalAppSecret":{"value":"'$SERVICE_PRINCIPAL_SECRET'"},
"workerVMSize":{"value":"Standard_D1_v2"},
"managerVMSize":{"value":"Standard_D1_v2"},
"enableSystemPrune":{"value":"yes"},
"sshPublicKey":{"value":"'$SSH_PUBLIC_KEY'"}}'

az group delete -n $GROUP_NAME -y
az group create --name $GROUP_NAME --location "West Europe"
az group deployment create \
--name dockertemplate \
--resource-group $GROUP_NAME \
--template-uri https://download.docker.com/azure/edge/Docker.tmpl \
--parameters "$PARAMETERS"

# ssh -p 50000 -fNL localhost:2374:/var/run/docker.sock docker@$SSH_IP
# export DOCKER_HOST=localhost:2374
# docker service create --name registry --publish 5000:5000 registry:2

# docker service create \
#   --publish=9090:8080 \
#   --limit-cpu 0.5 \
#   --name=viz \
#   --env PORT=9090 \
#   --constraint=node.role==manager \
#   --mount=type=bind,src=/var/run/docker.sock,dst=/var/run/docker.sock \
#   manomarks/visualizer

# #run CI/CD on master branch
# pushd ../../../src/awesome-ci
#     ./run.sh \
#         $REPO \
#         master \
#         localhost:5000/ \
#         $CI_TOKEN
# popd
