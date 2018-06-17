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
            steps {
                script {
                    checkout()
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
                expression { env.TAG_BRANCH = 'master' }
            }
            environment {
                QUIZ_ENVIRONMENT = 'staging'
            }
            steps {
                sh 'cd deploy && ./install.sh'
                sh 'cd deploy && ./tests.sh'
            }
        }

        stage('deploy to production') {
            when {
                expression { env.TAG_BRANCH = 'master' }
            }
            environment {
                QUIZ_ENVIRONMENT = 'pro'
            }
            steps {
                input 'Deploy to production?'
                sh 'cd deploy && ./install.sh'
            }
        }
    }
}

def checkout = {
    def scmVars = checkout scm

    env.TAG_COMMIT = smcVars.GIT_COMMIT
    env.TAG_BRANCH = scmVars.GIT_BRANCH
    env.TAG = "${scmVars.GIT_BRANCH}-${scmVars.GIT_COMMIT}"
}