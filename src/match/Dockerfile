FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Pitch.Match.API/Pitch.Match.API.csproj", "Pitch.Match.API/"]
COPY ["Pitch.Match.API.Tests/Pitch.Match.API.Tests.csproj", "Pitch.Match.API.Tests/"]
RUN dotnet restore "Pitch.Match.API/Pitch.Match.API.csproj"
RUN dotnet restore "Pitch.Match.API.Tests/Pitch.Match.API.Tests.csproj"
COPY . .
WORKDIR "/src/Pitch.Match.API"
RUN dotnet build "Pitch.Match.API.csproj" -c Release -o /app

FROM build AS testrunner
WORKDIR /src/Pitch.Match.API.Tests
COPY Pitch.Match.API.Tests/. .
ENTRYPOINT ["dotnet", "test"]
CMD ["--logger:trx", "/p:CollectCoverage=true", "/p:CoverletOutputFormat=cobertura", "/p:CoverletOutput=./TestResults/"]

FROM build AS publish
RUN dotnet publish "Pitch.Match.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Match.API.dll"]
