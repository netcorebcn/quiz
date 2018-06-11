pipeline {
    agent any
    environment {
        REGISTRY = credentials('registry')
        RABBIT_PASSWORD = credentials('postgres-password')
        POSTGRES_PASSWORD = credentials('rabbit-password')
    }

    stages {
        stage('checkout') {
            steps {
                github('netcorebcn/quiz')
                credentials('github-username')
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