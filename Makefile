all: clean restore build publish

clean:
	dotnet clean
	
restore:
	dotnet restore
	
build:
	dotnet build
	
publish:
	dotnet publish