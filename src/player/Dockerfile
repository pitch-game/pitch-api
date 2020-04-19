FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Pitch.Player.API/Pitch.Player.API.csproj", "Pitch.Player.API/"]
COPY ["Pitch.Player.API.Tests/Pitch.Player.API.Tests.csproj", "Pitch.Player.API.Tests/"]
RUN dotnet restore "Pitch.Player.API/Pitch.Player.API.csproj"
RUN dotnet restore "Pitch.Player.API.Tests/Pitch.Player.API.Tests.csproj"
COPY . .
WORKDIR "/src/Pitch.Player.API"
RUN dotnet build "Pitch.Player.API.csproj" -c Release -o /app

FROM build AS testrunner
WORKDIR /src/Pitch.Player.API.Tests
COPY Pitch.Player.API.Tests/. .
ENTRYPOINT ["dotnet", "test"]
CMD ["--logger:trx", "/p:CollectCoverage=true", "/p:CoverletOutputFormat=cobertura", "/p:CoverletOutput=./TestResults/"]

FROM build AS publish
RUN dotnet publish "Pitch.Player.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Player.API.dll"]
