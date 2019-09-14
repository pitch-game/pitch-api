FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.Identity.API/Pitch.Identity.API.csproj", "Pitch.Identity.API/"]
RUN dotnet restore "Pitch.Identity.API/Pitch.Identity.API.csproj"
COPY . .
WORKDIR "/src/Pitch.Identity.API"
RUN dotnet build "Pitch.Identity.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pitch.Identity.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Identity.API.dll"]
