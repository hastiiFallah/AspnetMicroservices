#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["services/Discount/DiscountGrpc/DiscountGrpc.csproj", "services/Discount/DiscountGrpc/"]
COPY ["BuildingBlocks/CommonLogging/CommonLogging.csproj", "BuildingBlocks/CommonLogging/"]
RUN dotnet restore "services/Discount/DiscountGrpc/DiscountGrpc.csproj"
COPY . .
WORKDIR "/src/services/Discount/DiscountGrpc"
RUN dotnet build "DiscountGrpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscountGrpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscountGrpc.dll"]