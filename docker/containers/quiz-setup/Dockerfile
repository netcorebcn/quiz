FROM microsoft/dotnet:runtime
WORKDIR /app
COPY build/quiz-setup .
ENTRYPOINT ["dotnet", "Quiz.EventSourcing.Setup.dll"]