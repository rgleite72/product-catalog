# ProductCatalog API

API para gestão de produtos e preços construída com:

* .NET 8
* PostgreSQL
* EF Core
* Docker

## 🔧 Como rodar local

### Pré-requisito

Copiar `.env.example` para `.env`

### 1️⃣ Subir infra
docker compose up -d


### 2️⃣ Aplicar migrations

dotnet ef database update \
  --project src/ProductCatalog.Infrastructure/ProductCatalog.Infrastructure.csproj \
  --startup-project src/ProductCatalog.Api/ProductCatalog.Api.csproj


### 3️⃣ Rodar API
dotnet run --project src/ProductCatalog.Api


## 🧪 Rodar testes
dotnet test


## 🐳 Rodar com Docker
docker compose up --build


## ❤️ Healthcheck
GET /health


## 📦 Endpoints

### Products
POST /api/products
GET /api/products
GET /api/products/{id}


### Prices
PUT /api/products/{id}/price

