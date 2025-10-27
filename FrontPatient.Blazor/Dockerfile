FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FrontPatient.Blazor/FrontPatient.Blazor.csproj", "FrontPatient.Blazor/"]
RUN dotnet restore "FrontPatient.Blazor/FrontPatient.Blazor.csproj"
COPY . .
WORKDIR "/src/FrontPatient.Blazor"
RUN dotnet build "./FrontPatient.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FrontPatient.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FrontPatient.Blazor.dll"]
