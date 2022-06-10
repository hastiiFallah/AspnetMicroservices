#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["services/Basket/BasketAPI/BasketAPI.csproj", "services/Basket/BasketAPI/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
RUN dotnet restore "services/Basket/BasketAPI/BasketAPI.csproj"
COPY . .
WORKDIR "/src/services/Basket/BasketAPI"
RUN dotnet build "BasketAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BasketAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasketAPI.dll"]