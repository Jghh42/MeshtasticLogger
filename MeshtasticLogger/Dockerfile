FROM mcr.microsoft.com/dotnet/aspnet:8.0.7-alpine3.20-arm64v8 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0.7-alpine3.20-arm64v8 AS build
WORKDIR /src
COPY ["MeshtasticLogger/MeshtasticLogger.csproj", "MeshtasticLogger/"]
RUN dotnet restore "MeshtasticLogger/MeshtasticLogger.csproj"
COPY . .
WORKDIR "/src/MeshtasticLogger"
RUN dotnet build "MeshtasticLogger.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MeshtasticLogger.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MeshtasticLogger.dll"]
