FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.Match.Api/Pitch.Match.Api.csproj", "Pitch.Match.Api/"]
COPY ["Pitch.Match.Api.Tests/Pitch.Match.Api.Tests.csproj", "Pitch.Match.Api.Tests/"]
RUN dotnet restore "Pitch.Match.Api/Pitch.Match.Api.csproj"
RUN dotnet restore "Pitch.Match.Api.Tests/Pitch.Match.Api.Tests.csproj"
COPY . .
WORKDIR "/src/Pitch.Match.Api"
RUN dotnet build "Pitch.Match.Api.csproj" -c Release -o /app

FROM build AS testrunner
WORKDIR /src/Pitch.Match.Api.Tests
COPY Pitch.Match.Api.Tests/. .
ENTRYPOINT ["dotnet", "test", "--logger:trx", "/p:CollectCoverage=true", "/p:CoverletOutputFormat=cobertura", "/p:CoverletOutput='./TestResults/'"]

FROM build AS publish
RUN dotnet publish "Pitch.Match.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Match.Api.dll"]
