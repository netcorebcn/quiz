# Quiz App
Simple EventSourcing example using .NET Core, React, Docker, Jenkins and K8s.

## Docker 
* run with [**docker**](https://www.docker.com/products/docker) from bash with ``.\run.sh`` 
  
  Open <http://localhost> for quiz voting and <http://localhost?results> for quiz results
  
## Minikube 
* run with [**minikube**](https://github.com/kubernetes/minikube)

  * Setup minikube   
  
    ```./setupkube.sh```

  * Setup dnsmasq (optional)

    ```sudo INGRESS_DOMAIN=quiz.internal ./setupdns.sh```

    **__Notes__**: For automatic dns wilcards resolution use [dnsmasq](https://blog.thesparktree.com/local-development-with-wildcard-dns)

  * Install jenkins and quiz app
    * Export the following environment variables:
    
      ```bash
      export INGRESS_DOMAIN='quiz.internal'
      export QUIZ_ENVIRONMENT='production'
      export TAG_BRANCH=master
      export REGISTRY=localhost:30400
      export TAG=latest

      export JENKINS_PASSWORD=changeit
      export GITHUB_REPO=netcorebcn/quiz
      export GITHUB_USER=mygithubuser
      export GITHUB_TOKEN='<TOKEN>'
      ```

    * Execute ```./install.sh```

    * Add ingress hosts to local host file (only if dnsmasq is not setup)
  
      ```echo $(minikube ip) {jenkins,rabbit,registry}.quiz.internal quiz.internal | sudo tee -a /etc/hosts```


    * Open <http://jenkins.quiz.internal/job/quiz/> and Build!

    * Once its build Open <http://quiz.internal> and <http://quiz.internal?results> 

    * Github integration for Pull Request workflow

      * Add Integration & Service: Manage Jenkins (GitHub plugin) 

        http://jenkins.quiz.internal/github-webhook/

      * For local jenkins integration you can use [ngrok](https://ngrok.com/) 
      
      ```bash 
      ./ngrok http jenkins.quiz.internal:80 -host-header=jenkins.quiz.internal
      ```
  ### Setup script example 
  There is an example of full setup script ```./fullsetup.sh```, it requires to store the enviroment variables in a secrets file.

## Notes
We aren't starting from the scratch. We are using ideas and code from other awesome repos.

* Running Jenkins in Docker

  <http://container-solutions.com/running-docker-in-jenkins-in-docker/>  

* K8s ingress configuration for ci cd with jenkins

  <https://github.com/kenzanlabs/kubernetes-ci-cd>

  <https://medium.com/@Oskarr3/setting-up-ingress-on-minikube-6ae825e98f82>

* Marten Event Store library for .NET and postgresql

  <https://github.com/JasperFx/marten>

* WebSockets helper classes thanks to  

  <https://github.com/radu-matei/websocket-manager>
  
* dnsmasq integrate with minikube

  <https://github.com/superbrothers/minikube-ingress-dns>
