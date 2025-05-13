FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["PetShop/PetShop.csproj", "PetShop/"]
COPY ["PetShop.Application/PetShop.Application.csproj", "PetShop.Application/"]
COPY ["PetShop.Domain/PetShop.Domain.csproj", "PetShop.Domain/"]
COPY ["PetShop.Infrastructure/PetShop.Infrastructure.csproj", "PetShop.Infrastructure/"]
RUN dotnet restore "PetShop/PetShop.csproj"
COPY . .
WORKDIR "/src/PetShop"
RUN dotnet build "PetShop.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PetShop.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetShop.dll"]
