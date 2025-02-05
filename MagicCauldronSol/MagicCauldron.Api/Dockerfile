FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MagicCauldron.Api/MagicCauldron.Api.csproj", "MagicCauldron.Api/"]
RUN dotnet restore "MagicCauldron.Api/MagicCauldron.Api.csproj"
COPY . .
WORKDIR "/src/MagicCauldron.Api"
RUN dotnet build "MagicCauldron.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MagicCauldron.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MagicCauldron.Api.dll"]
