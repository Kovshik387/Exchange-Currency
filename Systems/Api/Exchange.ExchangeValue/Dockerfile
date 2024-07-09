FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN apt-get update && apt-get install -y locales gnupg \
    && locale-gen ru_RU.UTF-8 \
    && apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 04EE7237B7D453EC \
    && apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 648ACFD622F3D138 \
    && apt-get update \
    && apt-get clean

ENV LANG=ru_RU.UTF-8
ENV LANGUAGE=ru_RU:ru
ENV LC_ALL=ru_RU.UTF-8

USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Systems/Api/Exchange.ExchangeValue/Exchange.ExchangeValute.csproj", "Systems/Api/Exchange.ExchangeValue/"]
COPY ["Data/Rate/Exchange.Context/Exchange.Context.csproj", "Data/Rate/Exchange.Context/"]
COPY ["Shared/Exchange.Common/Exchange.Common.csproj", "Shared/Exchange.Common/"]
COPY ["Data/Rate/Exchange.Entities/Exchange.Entities.csproj", "Data/Rate/Exchange.Entities/"]
COPY ["Data/Rate/Migrations/Exchange.Migrations.PgSql/Exchange.Context.Migrations.PgSql.csproj", "Data/Rate/Migrations/Exchange.Migrations.PgSql/"]
COPY ["Services/Exchange.Services.Logger/Exchange.Services.Logger.csproj", "Services/Exchange.Services.Logger/"]
COPY ["Services/Exchange.Services.Settings/Exchange.Services.Settings.csproj", "Services/Exchange.Services.Settings/"]
COPY ["Services/Exchange.Services.ValuteRate/Exchange.Services.ValutaRate.csproj", "Services/Exchange.Services.ValuteRate/"]
RUN dotnet restore "./Systems/Api/Exchange.ExchangeValue/Exchange.ExchangeValute.csproj"
COPY . .
WORKDIR "/src/Systems/Api/Exchange.ExchangeValue"
RUN dotnet build "./Exchange.ExchangeValute.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Exchange.ExchangeValute.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exchange.ExchangeValute.dll"]