
param(
    [string]$registry,
    [string]$sha="latest"
)

Write-Host "Registry:"$registry
Write-Host "Sha":$sha

#clean up
Remove-Item -Recurse -Force "build"
New-Item "build" -type directory

#build docker images
Get-ChildItem ./docker/containers -Directory | ForEach-Object {
    $containerPath = $_.FullName
    $container = $_.Name

    #run unit tests 
    if (Test-Path $containerPath\Dockerfile.tests)
    {
        docker build -t $container-tests:$sha -f $containerPath/Dockerfile.tests .
        if($error.length -gt 0){
            Exit 1
        }
    }

    if (Test-Path $containerPath\Dockerfile.build)
    {
        #build ci image
        docker build -t $container-ci:$sha -f $containerPath/Dockerfile.build .
        #publish build
        docker create --name $container-build-$sha $container-ci:$sha
        docker cp $container-build-${sha}:/build build/$container    
    }

    #build runtime image
    docker build -t $registry${container}:$sha -f $containerPath/Dockerfile .

    #push runtime image to registry
    if (![string]::IsNullOrEmpty($registry))
    {
        docker push $registry${container}:$sha
    }
}   

#clean up
docker system prune --force