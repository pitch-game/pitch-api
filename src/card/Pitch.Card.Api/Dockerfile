FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.Card.API/Pitch.Card.API.csproj", "Pitch.Card.API/"]
RUN dotnet restore "Pitch.Card.API/Pitch.Card.API.csproj"
COPY . .
WORKDIR "/src/Pitch.Card.API"
RUN dotnet build "Pitch.Card.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pitch.Card.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Card.API.dll"]
