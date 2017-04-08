# Quiz voting and results application
Example application with .NET Core, Event Store and Docker

* run with [**docker**](https://www.docker.com/products/docker)  
  ``.\run.sh``
  
  Open <http://localhost> for use quiz ui
  
  Open <http://localhost:81/swagger> for use quiz api
  
  Open <http://localhost:2113> for manage event store
  
**Notes**: We aren't started from the scratch and we don't reinvent wheels. We are using ideas and code from other awesome repos.

* Example architecture thanks to the popular   
  <https://github.com/docker/example-voting-app>

* EventStore repository thanks to [ReactiveTraderCloud](https://github.com/AdaptiveConsulting/ReactiveTraderCloud)

* WebSockets helper classes thanks to
  <https://github.com/radu-matei/websocket-manager>

* Docker swarm local cluster thanks to
  <https://codefresh.io/blog/deploy-docker-compose-v3-swarm-mode-cluster/>
  
  
