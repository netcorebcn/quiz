
param(
    [string]$BUILD_CONTEXT=".",
    [string]$TAG="latest",
    [string]$REGISTRY
)

Write-Host "BUILD_CONTEXT:"$BUILD_CONTEXT
Write-Host "TAG:"$TAG
Write-Host "REGISTRY:"$REGISTRY

$env:REGISTRY=$REGISTRY
$env:TAG=$TAG
$env:BUILD_CONTEXT=$BUILD_CONTEXT

docker-compose kill
docker-compose build

if (![string]::IsNullOrEmpty($REGISTRY))
{
    docker-compose push
}
