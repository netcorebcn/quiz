#clean up
rimraf build-results
rimraf build-voting
docker rm -f quiz-voting-build
docker rm -f quiz-results-build

#run unit tests 
docker build -t quiz-tests-ci -f ./docker/voting/Dockerfile.tests . || { echo "unit test failed"; exit 1; }

#build ci image
docker build -t quiz-voting-ci -f ./docker/voting/Dockerfile.build .
docker build -t quiz-results-ci -f ./docker/results/Dockerfile.build .

#publish build
docker create --name quiz-voting-build quiz-voting-ci
docker cp quiz-voting-build:/out build-voting

docker create --name quiz-results-build quiz-results-ci
docker cp quiz-results-build:/out build-results

#build runtime image
docker build -t quiz-voting -f ./docker/voting/Dockerfile .
docker build -t quiz-results -f ./docker/results/Dockerfile .

#clean up
docker system prune --force
