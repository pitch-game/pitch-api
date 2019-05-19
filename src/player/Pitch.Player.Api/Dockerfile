FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.Player.Api/Pitch.Player.Api.csproj", "Pitch.Player.Api/"]
RUN dotnet restore "Pitch.Player.Api/Pitch.Player.Api.csproj"
COPY . .
WORKDIR "/src/Pitch.Player.Api"
RUN dotnet build "Pitch.Player.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pitch.Player.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Player.Api.dll"]
