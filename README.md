# Quiz App
Simple EventSourcing example using .NET Core, React, Docker, Jenkins and K8s.

* run with [**docker**](https://www.docker.com/products/docker) from bash with ``.\run.sh`` 
  
  Open <http://localhost> for quiz voting
  
  Open <http://localhost?results> for quiz results
  
* run with [**minikube**](https://github.com/kubernetes/minikube)

  * Create secrets for jenkins, database, docker registry and messagebroker .

  ```bash
  DB_PASS=changeit \
  DB_USER=admin \
  DB_CONNECTION="Username=admin;Password=changeit;Host=db;Port=5432" \
  DOCKER_PASS=mydockerhubpass \
  DOCKER_USER=mydockerhubuser \
  GITHUB_TOKEN=1111111111111111111111 \
  JENKINS_PASS=changeit \
  JENKINS_USER=admin \
  MESSAGE_BROKER="amqp://guest:guest@messagebroker:5672" \
  ./secrets.sh
  ``` 
  
  * Build, push and deploy jenkins to k8s cluster
  
  ```bash
  eval $(minikube docker-env) && \
  REGISTRY=myregistry \
  GITHUB_REPO=netcorebcn\/quiz \
  TAG=latest \
  DOCKER_PASSWORD=$(cat secrets/docker-pass) \
  DOCKER_USER=myuser \
  JENKINS_URL=jenkins-url.com \
  GITHUB_ADMINS=mygithubuser \
  ./build-jenkins.sh
  ```

  * Add ingress hosts to local host file

  ```bash
  echo $(minikube ip) quiz{,-ci,-rabbit}.io | sudo tee -a /etc/hosts
  ```

  * Github integration

    * Add Integration & Service: Manage Jenkins (GitHub plugin) http://jenkins-url/github-webhook/

    * For local jenkins integration you can use [ngrok](https://ngrok.com/) 
    ```bash 
    ./ngrok http quiz-ci.io:80 -host-header=quiz-ci.io
    ```

  * Open <http://quiz-ci.io> for jenkins

  * Open <http://quiz.io> for quiz voting

  * Open <http://quiz.io?results> for quiz results


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
  
