#clean up
rimraf build
docker rm -f quiz-voting-build

#build ci image
docker build -t quiz-voting-ci -f ./docker/Dockerfile.build .

#publish build
docker create --name quiz-voting-build quiz-voting-ci
docker cp quiz-voting-build:/out build

#build runtime image
docker build -t quiz-voting -f ./docker/Dockerfile .

#clean up
docker system prune --force