FROM microsoft/dotnet:2.1-sdk-alpine AS builder
ARG api
ARG domain
ARG unitTests

# Restore packages
COPY ${api}*.csproj ${api}
COPY ${unitTests}*.csproj ${unitTests}
COPY ${domain}*.csproj ${domain}

RUN dotnet restore ${api} 
RUN dotnet restore ${unitTests} 

## Copy all sources
COPY ${api} ${api}
COPY ${unitTests} ${unitTests}
COPY ${domain} ${domain}

##### Run tests #####
RUN dotnet test ${unitTests} 

##### Publish project #####
RUN dotnet publish ${api} --output /publish --configuration Release 

# build runtime image from published release
FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine
WORKDIR /publish
COPY --from=builder /publish .
ENTRYPOINT ["dotnet", "Quiz.Queries.Api.dll"]
