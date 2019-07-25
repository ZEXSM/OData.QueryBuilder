ls

cd src/OData.QueryBuilder

dotnet minicover instrument --workdir ../../coverage --parentdir ../ --assemblies test/**/bin/Release/**/*.dll --sources src/**/*.cs

dotnet minicover reset --workdir ../../coverage

cd ../../

for project in test/**/*.csproj; do dotnet test --no-build $project; done

cd src/OData.QueryBuilder

dotnet minicover uninstrument --workdir ../../coverage

dotnet minicover report --workdir ../../coverage

cd ../../

# if [ -n "${TRAVIS_JOB_ID}" ]; then
# 	dotnet minicover coverallsreport \
# 		--workdir ../../coverage \
# 		--root-path ../../ \
# 		--output "coveralls.json" \
# 		--service-name "travis-ci" \
# 		--service-job-id "$TRAVIS_JOB_ID"
# fi