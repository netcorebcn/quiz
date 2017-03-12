FROM microsoft/dotnet:runtime
WORKDIR /app
COPY build/results .
ENTRYPOINT ["dotnet", "Quiz.Results.dll"]