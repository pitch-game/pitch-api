FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Pitch.Match.API/Pitch.Match.API.csproj", "Pitch.Match.API/"]
COPY ["Pitch.Match.Api.Tests.Unit/Pitch.Match.Api.Tests.Unit.csproj", "Pitch.Match.Api.Tests.Unit/"]
COPY ["Pitch.Match.Api.Tests.Functional/Pitch.Match.Api.Tests.Functional.csproj", "Pitch.Match.Api.Tests.Functional/"]
RUN dotnet restore "Pitch.Match.API/Pitch.Match.API.csproj"
RUN dotnet restore "Pitch.Match.Api.Tests.Unit/Pitch.Match.Api.Tests.Unit.csproj"
RUN dotnet restore "Pitch.Match.Api.Tests.Functional/Pitch.Match.Api.Tests.Functional.csproj"
COPY . .
WORKDIR "/src/Pitch.Match.API"
RUN dotnet build "Pitch.Match.API.csproj" -c Release -o /app

FROM build AS testrunner
WORKDIR /src/Pitch.Match.Api.Tests.Unit
COPY Pitch.Match.Api.Tests.Unit/. .
WORKDIR /src/Pitch.Match.Api.Tests.Functional
COPY Pitch.Match.Api.Tests.Functional/. .
ENTRYPOINT ["dotnet", "test"]
CMD ["--logger:trx", "/p:CollectCoverage=true", "/p:CoverletOutputFormat=cobertura", "/p:CoverletOutput=./TestResults/"]

FROM build AS publish
RUN dotnet publish "Pitch.Match.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Match.API.dll"]
