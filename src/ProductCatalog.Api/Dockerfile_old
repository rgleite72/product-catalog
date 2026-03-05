# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/ProductCatalog.Api/ProductCatalog.Api.csproj", "src/ProductCatalog.Api/"]
COPY ["src/ProductCatalog.Application/ProductCatalog.Application.csproj", "src/ProductCatalog.Application/"]
COPY ["src/ProductCatalog.Domain/ProductCatalog.Domain.csproj", "src/ProductCatalog.Domain/"]
COPY ["src/ProductCatalog.Infrastructure/ProductCatalog.Infrastructure.csproj", "src/ProductCatalog.Infrastructure/"]

RUN dotnet restore "src/ProductCatalog.Api/ProductCatalog.Api.csproj"

COPY . .
RUN dotnet publish "src/ProductCatalog.Api/ProductCatalog.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductCatalog.Api.dll"]
