FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Pitch.Match.Api/Pitch.Match.Api.csproj", "Pitch.Match.Api/"]
COPY ["Pitch.Match.Api.Tests.Unit/Pitch.Match.Api.Tests.Unit.csproj", "Pitch.Match.Api.Tests.Unit/"]
COPY ["Pitch.Match.Api.Tests.Functional/Pitch.Match.Api.Tests.Functional.csproj", "Pitch.Match.Api.Tests.Functional/"]
RUN dotnet restore "Pitch.Match.Api/Pitch.Match.Api.csproj"
RUN dotnet restore "Pitch.Match.Api.Tests.Unit/Pitch.Match.Api.Tests.Unit.csproj"
RUN dotnet restore "Pitch.Match.Api.Tests.Functional/Pitch.Match.Api.Tests.Functional.csproj"
COPY . .
WORKDIR "/src/Pitch.Match.Api"
RUN dotnet build "Pitch.Match.Api.csproj" -c Release -o /app

FROM build AS testrunner
WORKDIR /src
COPY . .
ENTRYPOINT ["dotnet", "test"]
CMD ["--logger:trx", "/p:CollectCoverage=true", "/p:CoverletOutputFormat=cobertura", "/p:CoverletOutput=./TestResults/"]

FROM build AS publish
RUN dotnet publish "Pitch.Match.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Match.Api.dll"]
