FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
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