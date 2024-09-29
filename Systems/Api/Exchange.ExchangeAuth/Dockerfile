FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Systems/Api/Exchange.ExchangeAuth/Exchange.ExchangeAuth.csproj", "Systems/Api/Exchange.ExchangeAuth/"]
RUN dotnet restore "Systems/Api/Exchange.ExchangeAuth/Exchange.ExchangeAuth.csproj"
COPY . .
WORKDIR "/src/Systems/Api/Exchange.ExchangeAuth"
RUN dotnet build "Exchange.ExchangeAuth.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Exchange.ExchangeAuth.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exchange.ExchangeAuth.dll"]
