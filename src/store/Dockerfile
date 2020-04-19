FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Pitch.Store.API/Pitch.Store.API.csproj", "Pitch.Store.API/"]
COPY ["Pitch.Store.API.Tests/Pitch.Store.API.Tests.csproj", "Pitch.Store.API.Tests/"]
RUN dotnet restore "Pitch.Store.API/Pitch.Store.API.csproj"
RUN dotnet restore "Pitch.Store.API.Tests/Pitch.Store.API.Tests.csproj"
COPY . .
WORKDIR "/src/Pitch.Store.API"
RUN dotnet build "Pitch.Store.API.csproj" -c Release -o /app

FROM build AS testrunner
WORKDIR /src/Pitch.Store.API.Tests
COPY Pitch.Store.API.Tests/. .
ENTRYPOINT ["dotnet", "test"]
CMD ["--logger:trx", "/p:CollectCoverage=true", "/p:CoverletOutputFormat=cobertura", "/p:CoverletOutput=./TestResults/"]

FROM build AS publish
RUN dotnet publish "Pitch.Store.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Store.API.dll"]
