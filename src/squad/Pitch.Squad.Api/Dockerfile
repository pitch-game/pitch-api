FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.Squad.Api/Pitch.Squad.Api.csproj", "Pitch.Squad.Api/"]
RUN dotnet restore "Pitch.Squad.Api/Pitch.Squad.Api.csproj"
COPY . .
WORKDIR "/src/Pitch.Squad.Api"
RUN dotnet build "Pitch.Squad.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pitch.Squad.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Squad.Api.dll"]
