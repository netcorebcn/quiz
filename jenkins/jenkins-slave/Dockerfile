FROM jenkins/jnlp-slave:3.19-1

USER root

# INSTALL DOCKER
RUN apt-get update && \
    apt-get install \
        apt-transport-https \
        ca-certificates \
        curl \
        python-pip \
        software-properties-common -y && \
    curl -fsSL https://download.docker.com/linux/debian/gpg | apt-key add - && \
    add-apt-repository \
        "deb [arch=amd64] https://download.docker.com/linux/debian \
        $(lsb_release -cs) \
        stable" && \ 
    apt-get update && \
    apt-get install docker-ce -y

# INSTALL DOCKER-COMPOSE
RUN curl -L --fail https://github.com/docker/compose/releases/download/1.21.2/docker-compose-Linux-x86_64 -o /usr/local/bin/docker-compose
RUN chmod +x /usr/local/bin/docker-compose
    
## INSTALL KUBECTL
RUN curl -LO https://storage.googleapis.com/kubernetes-release/release/`curl -s https://storage.googleapis.com/kubernetes-release/release/stable.txt`/bin/linux/amd64/kubectl
RUN chmod +x ./kubectl
RUN mv ./kubectl /usr/local/bin/kubectl

## INSTALL HELM
RUN curl https://raw.githubusercontent.com/kubernetes/helm/master/scripts/get > get_helm.sh
RUN chmod 700 get_helm.sh
RUN ./get_helm.sh
