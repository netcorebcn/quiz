// Jenkinsfile pipeline:
pipelineJob('pipeline') {
  definition {
    cpsScm {
      scm {
        github('netcorebcn/quiz')
      }
      scriptPath('Jenkinsfile')
    }
  }
}