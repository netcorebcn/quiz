
#!/bin/bash
export $(cat secrets)

kill  $(ps aux | grep ngrok | awk '{print $2}') 2> /dev/null 
./ngrok http jenkins.${INGRESS_DOMAIN}:80 -host-header=jenkins.${INGRESS_DOMAIN} -log=stdout > /dev/null & 
sleep 5
url=$(curl --silent http://127.0.0.1:4040/api/tunnels | jq '.tunnels[0].public_url')
export JENKINS_HOST=$(echo ${url//\"} | awk -F/ '{print $3}')
echo 'ngrok domain '${JENKINS_HOST}

sudo brew services stop dnsmasq
./setupkube.sh
sudo ./setupdns.sh
./install.sh