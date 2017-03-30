# Quiz voting and results application
Example application with .NET Core, Event Store and Docker

* run with [**docker**](https://www.docker.com/products/docker)
  
  ``.\build.sh``
  
  ``.\run.sh``
    
  
* run with [**dotnet**](https://github.com/dotnet/core/blob/master/release-notes/rc4-download.md)
  
  ``cd src/Quiz.Voting``
  
  ``dotnet restore``
  
  ``dotnet run``


The quiz voting api can be tested with swagger UI ``http://localhost:5000/swagger/`` and the [Event Store](https://geteventstore.com/) can be managed from ``http://localhost:2113``.


**Notes**: We aren't started from the scratch and we don't reinvent wheels. 
We are using ideas and code from other awesome repos.

* Example architecture thanks to the popular   
  ``https://github.com/docker/example-voting-app``

* EventStore repository thanks to [ReactiveTraderCloud](https://github.com/AdaptiveConsulting/ReactiveTraderCloud)

* WebSockets helper classes thanks to 
  ``https://github.com/radu-matei/websocket-manager``

* Docker swarm local cluster thanks to   
  ``https://codefresh.io/blog/deploy-docker-compose-v3-swarm-mode-cluster/``
