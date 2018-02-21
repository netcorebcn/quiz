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
  DOCKER_PASS=changeit \
  JENKINS_PASS=changeit \
  JENKINS_USER=admin \
  MESSAGE_BROKER="amqp://guest:guest@messagebroker:5672" \
  ./secrets.sh
  ``` 
  
  * Build, push and deploy jenkins to k8s cluster
  
  ```bash
  eval $(minikube docker-env) && \
  TAG=latest \
  ./build-jenkins.sh
  ```

  * Add ingress hosts to local host file

  ```bash
  echo $(minikube ip) quiz{,-ci,-rabbit}.io | sudo tee -a /etc/hosts
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
