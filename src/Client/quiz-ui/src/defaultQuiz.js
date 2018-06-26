export const defaultQuiz = {
    "questions": [
      {
        "description": "What is the most popular tool for running local k8s?",
        "options": [
          {
            "description": "superkube"
          },
          {
            "description": "minikube",
            "isCorrect": true
          },
          {
            "description": "dockerkube"
          }
        ]
      },
      {
        "description": "Which resource enables a virtual split of the k8s cluster?",
        "options": [
          {
            "description": "virtualvolumes"
          },
          {
            "description": "virtualhosts"
          },
          {
            "description": "namespaces",
            "isCorrect": true
          }
        ]
      },
      {
        "description": "What is the package manager for k8s?",
        "options": [
          {
            "description": "Helm",
            "isCorrect": true
          },
          {
            "description": "Nuget"
          },
          {
            "description": "kubeadm"
          }
        ]
      },
      {
        "description": "Which file defines the whole CI/CD process?",
        "options": [
          {
            "description": "Jenkinsfile",
            "isCorrect": true
          },
          {
            "description": "Dockerfile"
          },
          {
            "description": "docker-compose.yml"
          }
        ]
      },
      {
        "description": "Which docker feature enables building CI pipelines?",
        "options": [
          {
            "description": "Jenkinsfile"
          },
          {
            "description": "docker compose pipelines"
          },
          {
            "description": "multistage builds",
            "isCorrect": true
          }
        ]
      },
      {
        "description": "Which CLI tools the Jenkins slave needs to run the CI/CD pipeline?",
        "options": [
          {
            "description": "docker and minikube"
          },
          {
            "description": "docker and dotnet"
          },
          {
            "description": "docker, kubectl and helm",
            "isCorrect": true
          }
        ]
      }
    ]
}