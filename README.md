# Quiz voting and results application
Simple EventSourcing example using .NET Core, React, Docker and K8s.

* run with [**docker**](https://www.docker.com/products/docker) from bash with ``.\run.sh`` 
  
  Open <http://localhost:8080> for quiz voting
  
  Open <http://localhost:8080?results> for quiz results
  
  Open <http://localhost/swagger> for quiz api
  
  
**Notes**: We aren't starting from the scratch. We are using ideas and code from other awesome repos.

* Marten Event Store library for .NET and postgresql
  <https://github.com/JasperFx/marten>

* Example architecture thanks to the popular  
  <https://github.com/docker/example-voting-app>

* WebSockets helper classes thanks to  
  <https://github.com/radu-matei/websocket-manager>

* Running Jenkins in Docker
  <http://container-solutions.com/running-docker-in-jenkins-in-docker/>  

* K8s ingress configuration for ci cd with jenkins
  <https://github.com/kenzanlabs/kubernetes-ci-cd>
  <https://medium.com/@Oskarr3/setting-up-ingress-on-minikube-6ae825e98f82>