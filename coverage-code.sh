cd src/OData.QueryBuilder

# Instrument assemblies inside 'test' folder to detect hits for source files inside 'src' folder
dotnet minicover instrument --workdir ../../coverage --parentdir ../ --assemblies test/**/bin/Release/**/*.dll --sources src/**/*.cs

# Reset hits count in case minicover was run for this project
dotnet minicover reset --workdir ../../coverage

cd ../../

for project in test/**/*.csproj; do dotnet test --no-build $project; done

cd src/OData.QueryBuilder

# Uninstrument assemblies, it's important if you're going to publish or deploy build outputs
dotnet minicover uninstrument --workdir ../../coverage

dotnet minicover report --workdir ../../coverage

cd ../../
