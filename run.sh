docker rm -f quiz-voting-api
docker run --name quiz-voting-api -d -p 8080:80 quiz-voting