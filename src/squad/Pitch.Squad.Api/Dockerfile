FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.Squad.API/Pitch.Squad.API.csproj", "Pitch.Squad.API/"]
RUN dotnet restore "Pitch.Squad.API/Pitch.Squad.API.csproj"
COPY . .
WORKDIR "/src/Pitch.Squad.API"
RUN dotnet build "Pitch.Squad.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pitch.Squad.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Squad.API.dll"]
