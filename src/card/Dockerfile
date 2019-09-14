FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.Card.API/Pitch.Card.API.csproj", "Pitch.Card.API/"]
COPY ["Pitch.Card.API.Tests/Pitch.Card.API.Tests.csproj", "Pitch.Card.API.Tests/"]
RUN dotnet restore "Pitch.Card.API/Pitch.Card.API.csproj"
RUN dotnet restore "Pitch.Card.API.Tests/Pitch.Card.API.Tests.csproj"
COPY . .
WORKDIR "/src/Pitch.Card.API"
RUN dotnet build "Pitch.Card.API.csproj" -c Release -o /app

FROM build AS testrunner
WORKDIR /src/Pitch.Card.API.Tests
COPY Pitch.Card.API.Tests/. .
ENTRYPOINT ["dotnet", "test"]
CMD ["--logger:trx", "/p:CollectCoverage=true", "/p:CoverletOutputFormat=cobertura", "/p:CoverletOutput=./TestResults/"]

FROM build AS publish
RUN dotnet publish "Pitch.Card.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Card.API.dll"]
