FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["Pitch.Match.API.csproj", "./"]

RUN dotnet restore "./Pitch.Match.API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Pitch.Match.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pitch.Match.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.Match.API.dll"]