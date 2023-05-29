build_version := 1.0.$(shell date +%s)

all: dotnet_all docker_all

dotnet_all: clean restore build publish

clean:
	dotnet clean

restore:
	dotnet restore

build:
	dotnet build

publish:
	dotnet publish

docker_all: docker_build docker_push

docker_build:
	docker build . -f WebApi/Dockerfile -t learning_auth:$(build_version)

docker_push:
	docker image tag learning_auth:$(build_version) raspberry:5000/learning_auth:$(build_version)
	docker push raspberry:5000/learning_auth:$(build_version)