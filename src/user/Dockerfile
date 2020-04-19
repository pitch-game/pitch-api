FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Pitch.User.API/Pitch.User.API.csproj", "Pitch.User.API/"]
COPY ["Pitch.User.API.Tests/Pitch.User.API.Tests.csproj", "Pitch.User.API.Tests/"]
RUN dotnet restore "Pitch.User.API/Pitch.User.API.csproj"
RUN dotnet restore "Pitch.User.API.Tests/Pitch.User.API.Tests.csproj"
COPY . .
WORKDIR "/src/Pitch.User.API"
RUN dotnet build "Pitch.User.API.csproj" -c Release -o /app

FROM build AS testrunner
WORKDIR /src/Pitch.User.API.Tests
COPY Pitch.User.API.Tests/. .
ENTRYPOINT ["dotnet", "test"]
CMD ["--logger:trx", "/p:CollectCoverage=true", "/p:CoverletOutputFormat=cobertura", "/p:CoverletOutput=./TestResults/"]

FROM build AS publish
RUN dotnet publish "Pitch.User.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.User.API.dll"]
