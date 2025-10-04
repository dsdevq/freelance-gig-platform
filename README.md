# FreelanceGIG Platform

A microservices-based freelance platform built with .NET 9 and Clean Architecture.

## 🏗️ Architecture

This project follows a microservices architecture where each service is independent and can be deployed separately.

### Current Microservices
- **UserService**: User authentication and management

## 🚀 Running the Application

### Option 1: Docker Compose (Recommended)

Run all microservices and their dependencies in parallel:

```bash
docker-compose up --build
```

Run in background:
```bash
docker-compose up -d
```

Stop all services:
```bash
docker-compose down
```

Stop and remove volumes:
```bash
docker-compose down -v
```

### Option 2: Manual Startup (Local Development)

Run a single service for development:

```bash
dotnet run --project UserService/UserService.API
```

With hot reload:
```bash
dotnet watch --project UserService/UserService.API
```

### Option 3: Using Rider/Visual Studio

**JetBrains Rider:**
- Right-click on UserService.API project → Run/Debug

**Visual Studio:**
- Set UserService.API as startup project → F5

## 🌐 Service Endpoints

| Service | HTTP | HTTPS | Swagger |
|---------|------|-------|---------|
| UserService | http://localhost:5224 | https://localhost:7058 | https://localhost:7058/swagger |

## 📦 Project Structure

```
FreelanceGIGPlatform/
├── UserService/
│   ├── UserService.Domain/                # Domain entities and business logic
│   ├── UserService.Application/           # Application services and interfaces
│   ├── UserService.Infrastructure/        # Data access and external services
│   └── UserService.API/                   # Web API and endpoints
├── docker-compose.yml                     # Docker Compose for all services
├── README.md                              # This file
└── FreelanceGIGPlatform.sln              # Main solution (all projects)
```

## 🔧 Adding a New Microservice

1. Create the microservice folder structure:
```bash
mkdir YourService
cd YourService
```

2. Create projects:
```bash
dotnet new classlib -n YourService.Domain
dotnet new classlib -n YourService.Application
dotnet new classlib -n YourService.Infrastructure
dotnet new webapi -n YourService.API
```

3. Add all projects to main solution:
```bash
cd ..
dotnet sln add YourService/YourService.Domain/YourService.Domain.csproj
dotnet sln add YourService/YourService.Application/YourService.Application.csproj
dotnet sln add YourService/YourService.Infrastructure/YourService.Infrastructure.csproj
dotnet sln add YourService/YourService.API/YourService.API.csproj
```

4. Add to docker-compose.yml:
```yaml
  yourservice:
    build:
      context: ./YourService
      dockerfile: YourService.API/Dockerfile
    container_name: yourservice
    ports:
      - "5XXX:8080"
    networks:
      - freelance-network
```

## 🗄️ Database

Each microservice has its own database (database per service pattern):
- **UserService**: PostgreSQL on port 5432

## 🛠️ Development Tools

**Build all services:**
```bash
dotnet build
```

**Test all services:**
```bash
dotnet test
```

**Clean all services:**
```bash
dotnet clean
```

**Restore packages:**
```bash
dotnet restore
```

## 📝 Environment Variables

Key environment variables for each service should be set in:
- `appsettings.Development.json` for local development
- `docker-compose.yml` for containerized development
- Kubernetes ConfigMaps/Secrets for production

## 🔐 Security Notes

- Change default database passwords in production
- Use proper secret management (Azure Key Vault, AWS Secrets Manager, etc.)
- Configure CORS appropriately for your frontend
- Use HTTPS in production

## 📚 Technologies

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Docker & Docker Compose
- JWT Authentication

