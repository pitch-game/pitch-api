FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.Store.Api/Pitch.Store.Api.csproj", "Pitch.Store.Api/"]
RUN dotnet restore "Pitch.Store.Api/Pitch.Store.Api.csproj"
COPY . .
WORKDIR "/src/Pitch.Store.Api"
RUN dotnet build "Pitch.Store.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pitch.Store.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Store.Api.dll"]
