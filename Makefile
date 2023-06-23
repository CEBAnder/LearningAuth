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

docker_all: docker_build

docker_build:
	docker build . -f LearningAuth.Web/Dockerfile -t learning_auth:$(build_version)