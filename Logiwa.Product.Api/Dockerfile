FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Logiwa.Product.Api/Logiwa.Product.Api.csproj", "Logiwa.Product.Api/"]
RUN dotnet restore "Logiwa.Product.Api/Logiwa.Product.Api.csproj"
COPY . .
WORKDIR "/src/Logiwa.Product.Api"
RUN dotnet build "Logiwa.Product.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Logiwa.Product.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Logiwa.Product.Api.dll"]
