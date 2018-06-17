# DNSMASQ SETUP
domain=${INGRESS_DOMAIN:-'quiz.internal'}

# brew install dnsmasq
mkdir -p /usr/local/etc
rm -f /usr/local/etc/dnsmasq.conf
echo "bind-interfaces" | tee -a /usr/local/etc/dnsmasq.conf
echo "listen-address=127.0.0.1" | tee -a /usr/local/etc/dnsmasq.conf
echo address=/$domain/$(minikube ip) | tee -a /usr/local/etc/dnsmasq.conf

mkdir -p /etc/resolver
rm -f /etc/resolver/$domain
echo nameserver 127.0.0.1 | tee -a /etc/resolver/internal
brew services restart dnsmasq