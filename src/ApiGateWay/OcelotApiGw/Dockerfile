#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiGateWay/OcelotApiGw/OcelotApiGw.csproj", "ApiGateWay/OcelotApiGw/"]
COPY ["BuildingBlocks/CommonLogging/CommonLogging.csproj", "BuildingBlocks/CommonLogging/"]
RUN dotnet restore "ApiGateWay/OcelotApiGw/OcelotApiGw.csproj"
COPY . .
WORKDIR "/src/ApiGateWay/OcelotApiGw"
RUN dotnet build "OcelotApiGw.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OcelotApiGw.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OcelotApiGw.dll"]