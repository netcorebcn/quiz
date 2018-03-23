node {
    properties([pipelineTriggers([githubPush()])])
 
    stage "Checkout"
        checkout scm
        sh "git rev-parse --short HEAD > commit-id"
        env.TAG = readFile('commit-id').replace("\n", "").replace("\r", "")

    stage "Build"
        sh "./build.sh"

    stage "Deploy to staging"
        env.ENVIRONMENT="staging"
        sh "./deploy.sh"

    stage "Integration Tests"
        sh "./integration-tests.sh"
        
    stage "Deploy to production"
        env.ENVIRONMENT=""
        sh "./deploy.sh"
}