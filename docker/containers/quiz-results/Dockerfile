FROM microsoft/aspnetcore
WORKDIR /app
COPY build/quiz-results .
ENTRYPOINT ["dotnet", "Quiz.Results.dll"]