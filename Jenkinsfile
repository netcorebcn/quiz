node {
    stage "Checkout"
        checkout scm

        sh "git rev-parse --short HEAD > commit-id"
        env.TAG = readFile('commit-id').replace("\n", "").replace("\r", "")
        env.REGISTRY = 'paulopez'

    stage "Build"
        sh "./build.sh"

    stage "Deploy"
        sh "./deploy.sh"
}