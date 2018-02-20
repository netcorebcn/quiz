// Jenkinsfile pipeline:
pipelineJob('pipeline') {
  definition {
    cpsScm {
      scm {
        git {
            remote {
                github(System.getenv("GITHUB_REPO").trim())
                name('origin')
                refspec('+refs/pull/*:refs/remotes/origin/pr/*')
            }
            branch('${sha1}')
        }
      }
      triggers {
        githubPullRequest {
            admins(['paulopez78'])
            useGitHubHooks()
        }
    }
      scriptPath('Jenkinsfile')
    }
  }
}