FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Pitch.User.Api/Pitch.User.Api.csproj", "Pitch.User.Api/"]
RUN dotnet restore "Pitch.User.Api/Pitch.User.Api.csproj"
COPY . .
WORKDIR "/src/Pitch.User.Api"
RUN dotnet build "Pitch.User.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pitch.User.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pitch.User.Api.dll"]
