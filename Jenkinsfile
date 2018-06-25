pipeline {
    agent any
    environment {
        REGISTRY = 'localhost:30400'
        INGRESS_DOMAIN = 'quiz.internal'
    }

    stages {
        stage('build') {
            steps {
                script {
                    env.TAG_BRANCH = env.ghprbSourceBranch ?: 'master'
                    env.TAG_COMMIT = sh(returnStdout: true, script: "git log -n 1 --pretty=format:'%h'").trim()
                    env.TAG = "${env.TAG_BRANCH}-${env.TAG_COMMIT}"
                }
                sh './build.sh'
            }
        }

        stage('integration tests') {
            steps {
                sh 'cd deploy && ./tests.sh'
            }
        }

        stage('deploy to staging') {
            when {
                expression { env.TAG_BRANCH == 'master' }
            }
            environment {
                QUIZ_ENVIRONMENT = 'staging'
            }
            steps {
                sh 'cd deploy && ./install.sh'
            }
        }

        stage('deploy to production') {
            when {
                expression { env.TAG_BRANCH == 'master' }
            }
            environment {
                QUIZ_ENVIRONMENT = 'production'
            }
            steps {
                input 'Deploy to production?'
                sh 'cd deploy && ./install.sh'
            }
        }
    }
}