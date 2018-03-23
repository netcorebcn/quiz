# Quiz App
Simple EventSourcing example using .NET Core, React, Docker, Jenkins and K8s.

* run with [**docker**](https://www.docker.com/products/docker) from bash with ``.\run.sh`` 
  
  Open <http://localhost> for quiz voting
  
  Open <http://localhost?results> for quiz results
  
* run with [**minikube**](https://github.com/kubernetes/minikube)

  * Setup namespaces, secrets, jenkins and private registry

  ```bash
  export CLUSTER=minikube
  export CLUSTER_USER=minikube

  export DB_PASS=changeit
  export DB_USER=admin
  export DB_CONNECTION="Username=admin;Password=changeit;Host=db;Port=5432"
  export MESSAGE_BROKER="host=messagebroker:5672;username=guest;password=guest"

  export REGISTRY_PASS=
  export REGISTRY_USER=
  export REGISTRY=localhost:30400
  export TAG=latest

  export JENKINS_USER=admin
  export JENKINS_PASS=changeit
  export JENKINS_URL=jenkins-url.com

  export GITHUB_REPO=netcorebcn/quiz
  export GITHUB_ADMINS=mygithubuser
  export GITHUB_TOKEN='<github repo token>'
  
  eval $(minikube docker-env)
  ./setup.sh
  ```

  * Add ingress hosts to local host file

  ```bash
  echo $(minikube ip) quiz{,-ci,-rabbit,-rabbitstaging,staging}.io | sudo tee -a /etc/hosts
  ```

  * Open <http://quiz-ci.io/job/quiz-merge/> and Build!

  * Open <http://quiz.io> or <http://quizstaging.io> for quiz voting

  * Open <http://quiz.io?results> or <http://quizstaging.io?results> for quiz results


  * Github integration for Pull Request workflow

    * Add Integration & Service: Manage Jenkins (GitHub plugin) 

      http://jenkins-url/github-webhook/

    * For local jenkins integration you can use [ngrok](https://ngrok.com/) 
    
    ```bash 
    ./ngrok http quiz-ci.io:80 -host-header=quiz-ci.io
    ```

**Notes**: We aren't starting from the scratch. We are using ideas and code from other awesome repos.

* Running Jenkins in Docker

  <http://container-solutions.com/running-docker-in-jenkins-in-docker/>  

* K8s ingress configuration for ci cd with jenkins

  <https://github.com/kenzanlabs/kubernetes-ci-cd>

  <https://medium.com/@Oskarr3/setting-up-ingress-on-minikube-6ae825e98f82>

* Marten Event Store library for .NET and postgresql

  <https://github.com/JasperFx/marten>

* WebSockets helper classes thanks to  

  <https://github.com/radu-matei/websocket-manager>
  