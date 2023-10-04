build_version := 1.0.$(shell date +%s)

all: dotnet_all docker_all

dotnet_all: clean restore build publish test

clean:
	dotnet clean

restore:
	dotnet restore

build:
	dotnet build

publish:
	dotnet publish

test:
	dotnet test

docker_all: docker_build

docker_build:
	docker build . -f LearningAuth.Web/Dockerfile -t learning_auth:$(build_version)