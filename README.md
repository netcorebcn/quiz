# Quiz App
Simple EventSourcing example using .NET Core, React, Docker, Jenkins and K8s.

* run with [**docker**](https://www.docker.com/products/docker) from bash with ``.\run.sh`` 
  
  Open <http://localhost> for quiz voting and <http://localhost?results> for quiz results
  
* run with [**minikube**](https://github.com/kubernetes/minikube)

  * Start   
  
  ```minikube start --bootstrapper=kubeadm --memory=4096 --cpus=4 --vm-driver=hyperkit```

  * Install jenkins chart

  ```bash
  export REGISTRY=localhost:30400
  export TAG=latest

  export RABBIT_PASSWORD=changeit
  export POSTGRES_PASSWORD=changeit
  export JENKINS_PASSWORD=changeit

  export GITHUB_REPO=netcorebcn/quiz
  export GITHUB_ADMINS=mygithubuser
  export GITHUB_TOKEN='<TOKEN>'
  
  eval $(minikube docker-env)
  ./setup.sh
  ```

  * Add ingress hosts to local host file

  ```bash
  echo $(minikube ip) quiz{-ci,-rabbit}.io | sudo tee -a /etc/hosts
  ```

  * Open <http://quiz-ci.io/job/quiz-merge/> and Build!

  * Once its build Open <http://quiz.io> and <http://quiz.io?results> 


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
  