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
        sh "./k8s/quizapp/deploy.sh"

    stage "Integration Tests"
        sh "./k8s/quizapp/integration-tests.sh"
        
    stage "Deploy to production"
        env.ENVIRONMENT=""
        sh "./k8s/quizapp/deploy.sh"
}