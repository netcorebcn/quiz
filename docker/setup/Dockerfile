FROM microsoft/dotnet:runtime
WORKDIR /app
COPY build/setup .
ENTRYPOINT ["dotnet", "Quiz.EventSourcing.Setup.dll"]