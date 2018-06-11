pipeline {
    agent any
    environment {
        REGISTRY = 'localhost:30400'
        RABBIT_PASSWORD = credentials('postgres-password')
        POSTGRES_PASSWORD = credentials('rabbit-password')
    }

    stages {
        stage('checkout') {
            steps {
                checkout([
                    $class: 'GitSCM',
                    branches: [[name: '*/master']],
                    extensions: [
                        [$class: 'PruneStaleBranch'],
                        [$class: 'CleanCheckout'],
                    ],
                    userRemoteConfigs: [[
                        credentialsId: 'github-username', 
                        name: 'origin', 
                        url: 'https://github.com/netcorebcn/quiz']]
                ])
            }
        }
        stage('build') {
            steps {
                sh './build.sh'
            }
        }
        stage('integration tests') {
            steps {
                sh './integration-tests.sh'
            }
        }
        stage('deploy') {
            steps {
                sh './deploy.sh'
            }
        }
    }
}