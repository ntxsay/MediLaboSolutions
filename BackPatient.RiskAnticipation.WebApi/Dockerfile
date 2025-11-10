FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8090

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BackPatient.RiskAnticipation.WebApi/BackPatient.RiskAnticipation.WebApi.csproj", "BackPatient.RiskAnticipation.WebApi/"]
RUN dotnet restore "BackPatient.RiskAnticipation.WebApi/BackPatient.RiskAnticipation.WebApi.csproj"
COPY . .
WORKDIR "/src/BackPatient.RiskAnticipation.WebApi"
RUN dotnet build "./BackPatient.RiskAnticipation.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BackPatient.RiskAnticipation.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackPatient.RiskAnticipation.WebApi.dll"]
