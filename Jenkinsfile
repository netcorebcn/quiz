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
                checkout scm
            }
        }
        stage('build') {
            steps {
                sh './build.sh'
            }
        }
    }
}