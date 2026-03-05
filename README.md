# ProductCatalog API

API for product and price management built with:

* .NET 8
* ASP.NET Core
* PostgreSQL
* Entity Framework Core
* Docker
* Azure DevOps Pipelines (CI - restore/build)


## Overview

ProductCatalog is a backend API designed to manage products and pricing information using a layered architecture and practical, production-style development practices.

This repository focuses on:

* Product creation and maintenance
* Price registration by product
* Product inactivation
* Dockerized local environment (PostgreSQL)
* Initial CI pipeline (restore/build)


## Architecture

The solution follows a layered architecture:

### Domain

* Entities
* Business rules

### Application

* DTOs
* Service contracts
* Application services

### Infrastructure

* EF Core persistence
* Repository implementations
* Database configuration

### API

* Controllers
* Dependency Injection
* HTTP endpoints


## Project Structure

src/

 ├── ProductCatalog.API

 ├── ProductCatalog.Application

 ├── ProductCatalog.Domain

 ├── ProductCatalog.Infrastructure


## Main Features

### Product

* Create product
* Update product
* List products
* Get product by id
* Inactivate product

### Price

* Add price to product
* Retrieve product prices

## Main Endpoints

### Products

POST /api/products

GET /api/products

GET /api/products/{id}

PUT /api/products/{id}

PATCH /api/products/{id}/inactivate


### Prices

POST /api/products/{id}/prices

GET /api/products/{id}/prices


## Running Locally

### 1. Clone repository

git clone https://github.com/rgleite72/product-catalog.git
cd product-catalog


### 2. Start infrastructure

docker compose up -d


### 3. Apply migrations

dotnet ef database update --project src/ProductCatalog.Infrastructure --startup-project src/ProductCatalog.API


### 4. Run application

dotnet run --project src/ProductCatalog.API



## Docker

The project uses Docker Compose to provision PostgreSQL locally.

Main service:

* PostgreSQL database


## Continuous Integration

This project uses Azure DevOps Pipelines for Continuous Integration.

Pipeline executes:

* Restore dependencies
* Build solution


## Technology Stack

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* Docker
* Azure DevOps


## Status

Current version:

V1 - Product Catalog core + Docker + Initial CI

