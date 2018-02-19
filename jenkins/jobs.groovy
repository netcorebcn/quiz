// Jenkinsfile pipeline:
pipelineJob('pipeline') {
  definition {
    cpsScm {
      scm {
        github(System.getenv("GIT_REPO").trim())
      }
      scriptPath('Jenkinsfile')
    }
  }
}