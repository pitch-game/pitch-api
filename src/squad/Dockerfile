FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Pitch.Squad.API/Pitch.Squad.API.csproj", "Pitch.Squad.API/"]
COPY ["Pitch.Squad.API.Tests/Pitch.Squad.API.Tests.csproj", "Pitch.Squad.API.Tests/"]
RUN dotnet restore "Pitch.Squad.API/Pitch.Squad.API.csproj"
RUN dotnet restore "Pitch.Squad.API.Tests/Pitch.Squad.API.Tests.csproj"
COPY . .
WORKDIR "/src/Pitch.Squad.API"
RUN dotnet build "Pitch.Squad.API.csproj" -c Release -o /app

FROM build AS testrunner
WORKDIR /src/Pitch.Squad.API.Tests
COPY Pitch.Squad.API.Tests/. .
ENTRYPOINT ["dotnet", "test"]
CMD ["--logger:trx", "/p:CollectCoverage=true", "/p:CoverletOutputFormat=cobertura", "/p:CoverletOutput=./TestResults/"]

FROM build AS publish
RUN dotnet publish "Pitch.Squad.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Squad.API.dll"]
