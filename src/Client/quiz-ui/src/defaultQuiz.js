export const defaultQuiz = {
    "questions": [
      {
        "description": "What is the local k8s development?",
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
        "description": "Which feature enables splitting a k8s cluster?",
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
        "description": "Which is the managed option for using k8s in Azure?",
        "options": [
          {
            "description": "AKS",
            "isCorrect": true
          },
          {
            "description": "EKS"
          },
          {
            "description": "AK8s"
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
        "description": "Which CLI clients Jenkins needs to run the CI/CD pipeline?",
        "options": [
          {
            "description": "docker and minikube"
          },
          {
            "description": "docker and az"
          },
          {
            "description": "docker and kubectl",
            "isCorrect": true
          }
        ]
      }
    ]
};