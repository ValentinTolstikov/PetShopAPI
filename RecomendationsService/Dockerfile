FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 25000
EXPOSE 25001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RecomendationsService/RecomendationsService.csproj", "RecomendationsService/"]
RUN dotnet restore "RecomendationsService/RecomendationsService.csproj"
COPY . .
WORKDIR "/src/RecomendationsService"
RUN dotnet build "RecomendationsService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RecomendationsService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecomendationsService.dll"]
