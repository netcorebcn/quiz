param(
    [string]$BUILD_CONTEXT,
    [string]$TAG,
    [string]$REGISTRY
)

Write-Host "BUILD_CONTEXT:"$BUILD_CONTEXT
Write-Host "TAG:"$TAG
Write-Host "REGISTRY:"$REGISTRY

if (![string]::IsNullOrEmpty($BUILD_CONTEXT)) { $env:BUILD_CONTEXT=$BUILD_CONTEXT }
if (![string]::IsNullOrEmpty($TAG)) { $env:TAG=$TAG }
if (![string]::IsNullOrEmpty($REGISTRY)) { $env:REGISTRY=$REGISTRY }

docker-compose build
if (![string]::IsNullOrEmpty($REGISTRY)){ docker-compose push }
