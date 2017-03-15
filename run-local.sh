docker rm -f eventstore-node
docker run --name eventstore-node -d -p 2113:2113 -p 1113:1113 eventstore/eventstore

dotnet restore
dotnet run -p ./src/Quiz.EventSourcing.Setup/Quiz.EventSourcing.Setup.csproj &
dotnet run -p ./src/Quiz.Results/Quiz.Results.csproj &
dotnet run -p ./src/Quiz.Voting/Quiz.Voting.csproj &
