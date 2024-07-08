FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN apt-get update && apt-get install -y locales \
    && locale-gen ru_RU.UTF-8 \
    && apt-get clean

ENV LANG=ru_RU.UTF-8
ENV LANGUAGE=ru_RU:ru
ENV LC_ALL=ru_RU.UTF-8

USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Systems/Worker/Exchange.WorkerDaily/Exchange.WorkerDaily.csproj", "Systems/Worker/Exchange.WorkerDaily/"]
COPY ["Services/Exchange.Services.EmailAction/Exchange.Services.EmailAction.csproj", "Services/Exchange.Services.EmailAction/"]
COPY ["Shared/Exchange.Common/Exchange.Common.csproj", "Shared/Exchange.Common/"]
COPY ["Services/Exchange.Services.RabbitMq/Exchange.Services.RabbitMq.csproj", "Services/Exchange.Services.RabbitMq/"]
COPY ["Services/Exchange.Services.Settings/Exchange.Services.Settings.csproj", "Services/Exchange.Services.Settings/"]
COPY ["Services/Exchange.Services.Logger/Exchange.Services.Logger.csproj", "Services/Exchange.Services.Logger/"]
RUN dotnet restore "./Systems/Worker/Exchange.WorkerDaily/Exchange.WorkerDaily.csproj"
COPY . .
WORKDIR "/src/Systems/Worker/Exchange.WorkerDaily"
RUN dotnet build "./Exchange.WorkerDaily.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Exchange.WorkerDaily.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exchange.WorkerDaily.dll"]