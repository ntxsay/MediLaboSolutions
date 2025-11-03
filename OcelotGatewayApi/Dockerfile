FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8084

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OcelotGatewayApi/OcelotGatewayApi.csproj", "OcelotGatewayApi/"]
RUN dotnet restore "OcelotGatewayApi/OcelotGatewayApi.csproj"
COPY . .
WORKDIR "/src/OcelotGatewayApi"
RUN dotnet build "./OcelotGatewayApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OcelotGatewayApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY OcelotGatewayApi/ocelot.json .
ENTRYPOINT ["dotnet", "OcelotGatewayApi.dll"]
