// Jenkinsfile pipeline:
pipelineJob('quiz-pullrequest'){
  definition {
    cpsScm {
      scm {
        git {
            remote {
                github(System.getenv("GITHUB_REPO").trim())
                name('origin')
                refspec('+refs/pull/*:refs/remotes/origin/pr/*')
            }
            branch('${ghprbActualCommit}')
        }
      }
      triggers {
        githubPullRequest {
            admins([System.getenv("GITHUB_ADMINS").trim()])
            useGitHubHooks()
        }
      }
      scriptPath('Jenkinsfile.pr')
    }
  }
}
pipelineJob('quiz-merge') {
  definition {
    cpsScm {
      scm {
          github(System.getenv("GITHUB_REPO").trim())
      }
      triggers {
        githubPush()
      }
      scriptPath('Jenkinsfile')
    }
  }
}