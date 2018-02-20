FROM jenkins/jenkins:lts

USER root

# INSTALL DOCKER
RUN apt-get update && \
    apt-get install \
        apt-transport-https \
        ca-certificates \
        curl \
        software-properties-common -y && \
    curl -fsSL https://download.docker.com/linux/debian/gpg | apt-key add - && \
    add-apt-repository \
        "deb [arch=amd64] https://download.docker.com/linux/debian \
        $(lsb_release -cs) \
        stable" && \ 
    apt-get update && \
    apt-get install docker-ce -y
    
RUN curl -L --fail https://github.com/docker/compose/releases/download/1.18.0/docker-compose-Linux-x86_64 -o /usr/local/bin/docker-compose
RUN chmod +x /usr/local/bin/docker-compose
    
## INSTALL KUBECTL
RUN curl -LO https://storage.googleapis.com/kubernetes-release/release/`curl -s https://storage.googleapis.com/kubernetes-release/release/stable.txt`/bin/linux/amd64/kubectl
RUN chmod +x ./kubectl
RUN mv ./kubectl /usr/local/bin/kubectl

# Whether to skip setup wizard
ENV JAVA_OPTS="-Djenkins.install.runSetupWizard=false"

COPY security.groovy /usr/share/jenkins/ref/init.groovy.d/security.groovy
COPY ghprb_auth.groovy /usr/share/jenkins/ref/init.groovy.d/ghprb_auth.groovy
COPY seedJob.groovy /usr/share/jenkins/ref/init.groovy.d/seedJob.groovy

COPY plugins.txt /usr/share/jenkins/ref/plugins.txt
RUN /usr/local/bin/install-plugins.sh < /usr/share/jenkins/ref/plugins.txt

COPY jobs.groovy /usr/share/jenkins/jobs.groovy
