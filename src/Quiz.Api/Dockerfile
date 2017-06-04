FROM microsoft/aspnetcore-build:2 AS build-image

# copy shared libraries
COPY src/Quiz.Domain /src/Quiz.Domain

# restore packages creating caching layer 
WORKDIR /test/Quiz.Domain.Tests
COPY test/Quiz.Domain.Tests/Quiz.Domain.Tests.csproj .
RUN dotnet restore

# run units
COPY test/Quiz.Domain.Tests .
RUN dotnet test

# restore packages creating caching layer 
WORKDIR /src/Quiz.Api
COPY src/Quiz.Api/Quiz.Api.csproj .
RUN dotnet restore

# publish release
COPY src/Quiz.Api .
RUN dotnet publish  --output /build/ --configuration Release

# build runtime image from published release
FROM microsoft/aspnetcore:2
WORKDIR /app
COPY --from=build-image build/ .
ENTRYPOINT ["dotnet", "Quiz.Api.dll"]
