#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["services/Ordering/OrderingAPI/OrderingAPI.csproj", "services/Ordering/OrderingAPI/"]
COPY ["BuildingBlocks/CommonLogging/CommonLogging.csproj", "BuildingBlocks/CommonLogging/"]
COPY ["services/Ordering/OrderingApplication/OrderingApplication.csproj", "services/Ordering/OrderingApplication/"]
COPY ["services/Ordering/OrderingDomain/OrderingDomain.csproj", "services/Ordering/OrderingDomain/"]
COPY ["services/Ordering/OrderingInfrastructure/OrderingInfrastructure.csproj", "services/Ordering/OrderingInfrastructure/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
RUN dotnet restore "services/Ordering/OrderingAPI/OrderingAPI.csproj"
COPY . .
WORKDIR "/src/services/Ordering/OrderingAPI"
RUN dotnet build "OrderingAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderingAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderingAPI.dll"]