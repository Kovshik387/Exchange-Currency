#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Systems/Api/Exchange.ExchangeAccount/Exchange.ExchangeAccount.csproj", "Systems/Api/Exchange.ExchangeAccount/"]
COPY ["Data/Account/Exchange.Account.Context/Exchange.Account.Context.csproj", "Data/Account/Exchange.Account.Context/"]
COPY ["Services/Exchange.Services.Settings/Exchange.Services.Settings.csproj", "Services/Exchange.Services.Settings/"]
COPY ["Shared/Exchange.Common/Exchange.Common.csproj", "Shared/Exchange.Common/"]
COPY ["Data/Account/Exchange.Account.Entities/Exchange.Account.Entities.csproj", "Data/Account/Exchange.Account.Entities/"]
COPY ["Services/Exchange.Services.Authorization/Exchange.Services.Authorization.csproj", "Services/Exchange.Services.Authorization/"]
RUN dotnet restore "./Systems/Api/Exchange.ExchangeAccount/Exchange.ExchangeAccount.csproj"
COPY . .
WORKDIR "/src/Systems/Api/Exchange.ExchangeAccount"
RUN dotnet build "./Exchange.ExchangeAccount.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Exchange.ExchangeAccount.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exchange.ExchangeAccount.dll"]