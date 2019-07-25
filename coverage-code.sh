#!/usr/bin/env bash

cd /src/OData.QueryBuilder
dotnet minicover instrument --workdir ../../coverage --parentdir ../ --assemblies test/**/bin/${CONFIGURATION}/**/*.dll --sources src/**/*.cs
dotnet minicover reset --workdir ../../coverage
cd ../../
dotnet test --no-build test/**/*.csproj
cd /src/OData.QueryBuilder
dotnet minicover uninstrument --workdir ../../coverage
dotnet minicover report --workdir ../../coverage
dotnet minicover coverallsreport --workdir ../../coverage --root-path ../../ --output "coveralls.json" --service-name "travis-ci" --service-job-id $TRAVIS_JOB_ID
cd ../../