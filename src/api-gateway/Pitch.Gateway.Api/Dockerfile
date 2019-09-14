FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.Gateway.API/Pitch.Gateway.API.csproj", "Pitch.Gateway.API/"]
RUN dotnet restore "Pitch.Gateway.API/Pitch.Gateway.API.csproj"
COPY . .
WORKDIR "/src/Pitch.Gateway.API"
RUN dotnet build "Pitch.Gateway.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pitch.Gateway.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Gateway.API.dll"]

HEALTHCHECK CMD curl --fail http://localhost:5000/health || exit