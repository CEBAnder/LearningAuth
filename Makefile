build_version = $(shell date +%s)

all: clean restore build publish build_docker_image

clean:
	dotnet clean
	
restore:
	dotnet restore
	
build:
	dotnet build
	
publish:
	dotnet publish
	
build_docker_image:
	docker build . -f WebApi/Dockerfile -t learning_auth:1.0.$(build_version)