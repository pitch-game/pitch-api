FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.Identity.API/Pitch.Identity.API.csproj", "Pitch.Identity.API/"]
COPY ["Pitch.Identity.API.Tests/Pitch.Identity.API.Tests.csproj", "Pitch.Identity.API.Tests/"]
RUN dotnet restore "Pitch.Identity.API/Pitch.Identity.API.csproj"
RUN dotnet restore "Pitch.Identity.API.Tests/Pitch.Identity.API.Tests.csproj"
COPY . .
WORKDIR "/src/Pitch.Identity.API"
RUN dotnet build "Pitch.Identity.API.csproj" -c Release -o /app

FROM build AS testrunner
WORKDIR /src/Pitch.Identity.API.Tests
COPY Pitch.Identity.API.Tests/. .
ENTRYPOINT ["dotnet", "test"]
CMD ["--logger:trx", "/p:CollectCoverage=true", "/p:CoverletOutputFormat=cobertura", "/p:CoverletOutput=./TestResults/"]

FROM build AS publish
RUN dotnet publish "Pitch.Identity.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Identity.API.dll"]
