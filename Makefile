test:
	dotnet test

dev:
	dotnet watch test --project Test/Test.fsproj

clean:
	rm -rf ./*/bin ./*/obj fic-fac-foe
	dotnet restore

fic-fac-foe:
	cp $(shell dotnet publish | tail -n 1 | cut -d'>' -f2)/Cli fic-fac-foe