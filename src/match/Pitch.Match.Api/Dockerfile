FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY *.sln .
COPY ["Pitch.Match.Api/Pitch.Match.Api.csproj", "Pitch.Match.Api/"]
COPY ["Pitch.Match.Api.Tests/Pitch.Match.Api.Tests.csproj", "Pitch.Match.Api.Tests/"]
RUN dotnet restore
COPY . .
WORKDIR "/src/Pitch.Match.Api"
RUN dotnet build "Pitch.Match.Api.csproj" -c Release -o /app

FROM build AS testrunner
WORKDIR /app/Pitch.Match.Api.Tests
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

FROM build AS test
WORKDIR /app/Pitch.Match.Api.Tests
RUN dotnet test

FROM build AS publish
RUN dotnet publish "Pitch.Match.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Match.Api.dll"]
