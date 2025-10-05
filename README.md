# FreelanceGIG Platform

A microservices-based freelance platform built with .NET 9.

## Architecture

This project follows Clean Architecture and Microservices patterns with shared infrastructure:

```
├── Shared/                          # Shared libraries (reusable across services)
│   ├── Shared.Application/          # Pure interfaces, no dependencies
│   ├── Shared.Infrastructure/       # EF Core implementations
│   └── Shared.WebApi/              # ASP.NET Core extensions (JWT, Swagger)
│
├── UserService/                     # User management microservice
│   ├── UserService.Domain/
│   ├── UserService.Application/
│   ├── UserService.Infrastructure/
│   └── UserService.API/
│
└── JobService/                      # Job management microservice
    ├── JobService.Domain/
    ├── JobService.Application/
    ├── JobService.Infrastructure/
    └── JobService.API/
```

## Getting Started

### Prerequisites

- .NET 9 SDK
- Docker & Docker Compose (for containerized deployment)
- PostgreSQL (if running locally)

### Configuration

1. **Copy the environment file:**
   ```bash
   cp .env.example .env
   ```

2. **Update JWT settings** in `.env` (optional for development):
   ```env
   JWT_SECRET_KEY=your-secret-key-here
   JWT_ISSUER=FreelanceGIGPlatform.API
   JWT_AUDIENCE=FreelanceGIGPlatform.Client
   JWT_EXPIRY_HOURS=1
   ```

   > **Note:** Environment variables in `.env` are automatically loaded by Docker Compose. The services will use these shared JWT settings, avoiding configuration duplication.

### Running with Docker Compose

```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop all services
docker-compose down
```

Services will be available at:
- **UserService API**: http://localhost:5224, https://localhost:7058
- **Swagger UI**: http://localhost:5224/swagger
- **JobService API**: http://localhost:5225, https://localhost:7059
- **Swagger UI**: http://localhost:5225/swagger

### Running Locally (Development)

1. **Start PostgreSQL databases** (or use Docker Compose for just databases):
   ```bash
   docker-compose up -d userservice_db jobservice_db
   ```

2. **Run UserService:**
   ```bash
   cd UserService/UserService.API
   dotnet run
   ```

3. **Run JobService:**
   ```bash
   cd JobService/JobService.API
   dotnet run
   ```

## Project Features

### Shared Infrastructure

✅ **No package leakage** - Application layers only reference lightweight interfaces  
✅ **Unified service registration** - One-liner extension methods  
✅ **Single source of truth** - No duplicate interfaces or implementations  
✅ **Proper separation of concerns** - Clean Architecture boundaries respected

### Shared Components

- **Persistence Layer:**
  - `IUnitOfWork` & `ITransaction` interfaces
  - `UnitOfWorkBase<TDbContext>` - Generic EF Core implementation
  - `EfCoreTransaction` - Transaction wrapper

- **Web API Layer:**
  - `AddSharedJwtAuthentication()` - JWT authentication setup
  - `AddSharedSwagger()` / `UseSharedSwagger()` - Swagger with JWT support

### Configuration Management

JWT settings are centralized:
- **Development**: Use `appsettings.json` or `.env` file
- **Docker**: Environment variables from `.env` (loaded by docker-compose)
- **Production**: Use environment variables, Kubernetes ConfigMaps/Secrets, or Azure Key Vault

## API Documentation

Once the services are running, visit:
- UserService Swagger: http://localhost:5224/swagger
- JobService Swagger: http://localhost:5225/swagger

## Database Migrations

### UserService
```bash
cd UserService/UserService.Infrastructure
dotnet ef migrations add MigrationName -s ../UserService.API
dotnet ef database update -s ../UserService.API
```

### JobService
```bash
cd JobService/JobService.Infrastructure
dotnet ef migrations add MigrationName -s ../JobService.API
dotnet ef database update -s ../JobService.API
```

## Testing

```bash
# Run all tests
dotnet test

# Run specific project tests
dotnet test UserService/UserService.Tests
```

## Best Practices

1. **JWT Configuration**: Update `JWT_SECRET_KEY` in production. Use a strong, randomly generated key (at least 256 bits).

2. **Database Passwords**: Never commit real passwords. Use environment variables or secret management tools.

3. **Microservices Independence**: Each service has its own database and can be deployed independently.

4. **Shared Code**: Only share infrastructure and application-level abstractions. Domain logic stays in each service.

## License

[Your License Here]