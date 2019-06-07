FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.Gateway.Api/Pitch.Gateway.Api.csproj", "Pitch.Gateway.Api/"]
RUN dotnet restore "Pitch.Gateway.Api/Pitch.Gateway.Api.csproj"
COPY . .
WORKDIR "/src/Pitch.Gateway.Api"
RUN dotnet build "Pitch.Gateway.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pitch.Gateway.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Gateway.Api.dll"]

HEALTHCHECK CMD curl --fail http://localhost:5000/health || exit