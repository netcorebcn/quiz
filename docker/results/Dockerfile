FROM microsoft/aspnetcore
WORKDIR /app
COPY build/results .
ENTRYPOINT ["dotnet", "Quiz.Results.dll"]