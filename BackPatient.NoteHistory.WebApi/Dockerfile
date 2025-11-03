FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8088

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BackPatient.NoteHistory.WebApi/BackPatient.NoteHistory.WebApi.csproj", "BackPatient.NoteHistory.WebApi/"]
RUN dotnet restore "BackPatient.NoteHistory.WebApi/BackPatient.NoteHistory.WebApi.csproj"
COPY . .
WORKDIR "/src/BackPatient.NoteHistory.WebApi"
RUN dotnet build "./BackPatient.NoteHistory.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BackPatient.NoteHistory.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackPatient.NoteHistory.WebApi.dll"]
