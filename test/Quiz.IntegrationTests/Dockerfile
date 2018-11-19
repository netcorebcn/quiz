FROM microsoft/dotnet:2.1-sdk-alpine AS builder 
ARG tests
ARG commands
ARG queries
ARG domain

# Restore packages
COPY ${tests}*.csproj ${tests}
COPY ${commands}*.csproj ${commands}
COPY ${queries}*.csproj ${queries}
COPY ${domain}*.csproj ${domain}

RUN dotnet restore ${tests}

## Copy all sources
COPY ${tests} ${tests}
COPY ${commands} ${commands}
COPY ${queries} ${queries}
COPY ${domain} ${domain}

WORKDIR ${tests}
RUN dotnet build