FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.User.API/Pitch.User.API.csproj", "Pitch.User.API/"]
RUN dotnet restore "Pitch.User.API/Pitch.User.API.csproj"
COPY . .
WORKDIR "/src/Pitch.User.API"
RUN dotnet build "Pitch.User.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pitch.User.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.User.API.dll"]
