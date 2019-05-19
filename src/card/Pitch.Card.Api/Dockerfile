FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.Card.Api/Pitch.Card.Api.csproj", "Pitch.Card.Api/"]
RUN dotnet restore "Pitch.Card.Api/Pitch.Card.Api.csproj"
COPY . .
WORKDIR "/src/Pitch.Card.Api"
RUN dotnet build "Pitch.Card.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pitch.Card.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Card.Api.dll"]
