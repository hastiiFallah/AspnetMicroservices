
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["services/Discount/DiscountAPI/DiscountAPI.csproj", "services/Discount/DiscountAPI/"]
RUN dotnet restore "services/Discount/DiscountAPI/DiscountAPI.csproj"
COPY . .
WORKDIR "/src/services/Discount/DiscountAPI"
RUN dotnet build "DiscountAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscountAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscountAPI.dll"]
