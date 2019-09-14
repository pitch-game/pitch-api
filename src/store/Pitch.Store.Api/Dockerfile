FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.Store.API/Pitch.Store.API.csproj", "Pitch.Store.API/"]
RUN dotnet restore "Pitch.Store.API/Pitch.Store.API.csproj"
COPY . .
WORKDIR "/src/Pitch.Store.API"
RUN dotnet build "Pitch.Store.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pitch.Store.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Store.API.dll"]
