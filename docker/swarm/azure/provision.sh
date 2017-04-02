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

