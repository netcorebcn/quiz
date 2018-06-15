pipeline {
    agent any
    environment {
        REGISTRY = 'localhost:30400'
        RABBIT_PASSWORD = credentials('postgres-password')
        POSTGRES_PASSWORD = credentials('rabbit-password')
        INGRESS_DOMAIN = 'quiz.internal'
    }

    stages {
        stage('build') {
            script {
                checkout()
            }
            steps {
                sh './build.sh'
            }
        }
        stage('integration tests') {
            steps {
                sh 'cd deploy && ./tests.sh'
            }
        }
        stage('deploy') {
            steps {
                sh 'cd deploy && ./install.sh'
            }
        }
    }
}

def checkout = {
    tagBranch = 'master'
    repo = 'https://github.com/netcorebcn/quiz.git'

    tagBranch = env.BRANCH_NAME ?: tagBranch
    checkout([
        $class: 'GitSCM',
        branches: [[name: tagBranch]],
        extensions: [
            [$class: 'CleanCheckout'],
        ],
        userRemoteConfigs: [[name: 'origin', url: "${repo}"]]
    ])

    env.TAG_COMMIT = sh(returnStdout: true, script: "git log -n 1 --pretty=format:'%h'").trim()
    env.TAG_BRANCH = tagBranch
    env.TAG = "${env.TAG_BRANCH}-${env.TAG_COMMIT}"
}