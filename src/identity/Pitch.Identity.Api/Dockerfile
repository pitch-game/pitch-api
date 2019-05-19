FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.Identity.Api/Pitch.Identity.Api.csproj", "Pitch.Identity.Api/"]
RUN dotnet restore "Pitch.Identity.Api/Pitch.Identity.Api.csproj"
COPY . .
WORKDIR "/src/Pitch.Identity.Api"
RUN dotnet build "Pitch.Identity.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pitch.Identity.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Identity.Api.dll"]
