FROM microsoft/aspnetcore-build:2.0.3 AS builder
ARG api
ARG domain
ARG unitTests

# Restore packages
COPY ${api}/*.csproj ${api}/
COPY ${unitTests}/*.csproj ${unitTests}/
COPY ${domain}/*.csproj ${domain}/

RUN dotnet restore /${api}
RUN dotnet restore /${unitTests}

## Copy all sources
COPY ${api} /${api}
COPY ${unitTests} /${unitTests}
COPY ${domain} /${domain}

##### Run tests #####
RUN dotnet test /${unitTests} --configuration Release

##### Publish project #####
RUN dotnet publish /${api} --output /publish --configuration Release


# build runtime image from published release
FROM microsoft/aspnetcore
WORKDIR /publish
COPY --from=builder /publish .
ENTRYPOINT ["dotnet", "Quiz.Queries.Api.dll"]
