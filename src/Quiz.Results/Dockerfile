FROM microsoft/aspnetcore-build AS build-image

# copy shared libraries
COPY src/Quiz.Domain /src/Quiz.Domain

# restore packages creating caching layer 
WORKDIR /src/Quiz.Results
COPY src/Quiz.Results/Quiz.Results.csproj .
RUN dotnet restore

# publish release
COPY src/Quiz.Results .
RUN dotnet publish  --output /build/ --configuration Release

# build runtime image from published release
FROM microsoft/aspnetcore
WORKDIR /app
COPY --from=build-image build/ .
ENTRYPOINT ["dotnet", "Quiz.Results.dll"]