# Restaurants

A .NET 8 clean architecture sample application for restaurant management.

This repository contains a modular solution that implements:

- A REST API for restaurants, dishes and user identity management
- ASP.NET Core minimal APIs combined with MVC controllers
- Domain-driven design with Application, Domain and Infrastructure layers
- Authentication, authorization and policy-based access control
- Azure Blob Storage integration for restaurant logo uploads
- Entity Framework Core persistence with SQL Server and database seeding
- Swagger UI for API exploration and documentation
- Serilog logging with optional Application Insights support

## Solution structure

- `src/Restaurants.API` — ASP.NET Core web API application
- `src/Restaurants.Application` — application layer with MediatR request handlers, commands, queries and DTOs
- `src/Restaurants.Domain` — domain entities, enums, constants and identity models
- `src/Restaurants.Infrastructure` — persistence, repository implementations, authorization, blob storage and EF Core migrations
- `tests` — unit/integration tests for API, application, and infrastructure layers

## Core features

- Restaurant management: create, update, delete, list and retrieve restaurant data
- Dish management: create dishes, list dishes by restaurant, get dish details and remove restaurant dishes
- Logo upload: upload restaurant logo images to Azure Blob Storage
- Identity management: update user details, assign and unassign roles
- Authorization policies: nationality-based access, minimum age requirements, owner role enforcement
- Global error handling and request time logging

## API endpoints

### Restaurants

- `GET /api/restaurants` — list restaurants
- `GET /api/restaurants/{id}` — get restaurant details
- `POST /api/restaurants` — create a restaurant
- `PATCH /api/restaurants/{id}` — update a restaurant
- `DELETE /api/restaurants/{id}` — delete a restaurant
- `POST /api/restaurants/{id}/logo` — upload a restaurant logo

### Dishes

- `POST /api/restaurants/{restaurantId}/dishes` — add a new dish to a restaurant
- `GET /api/restaurants/{restaurantId}/dishes` — list dishes for a restaurant
- `GET /api/restaurants/{restaurantId}/dishes/{dishId}` — get dish details
- `DELETE /api/restaurants/{restaurantId}/dishes` — delete all dishes for a restaurant

### Identity

- `PATCH /api/identity/user` — update logged-in user details
- `POST /api/identity/userRole` — assign a role to a user
- `DELETE /api/identity/userRole` — remove a role from a user

## Development setup

### Prerequisites

- .NET 8 SDK
- Docker (for local SQL Server container)
- Azure Storage account credentials if using blob storage

### Local environment

1. Start the local database environment:
   ```bash
   docker compose up -d database
   ./scripts/environment/dev.sh
   ```
2. Build and run the API:
   ```bash
   dotnet build Restaurants.sln
   dotnet run --project src/Restaurants.API/Restaurants.API.csproj
   ```
3. Open Swagger UI in a browser when the app is running (default behavior in Development):
   - `https://localhost:5001/swagger`

### Configuration

The API reads configuration from `appsettings.json`, `appsettings.Development.json`, and environment variables.

Important configuration sections:

- `ConnectionStrings:RestaurantsDb` — SQL Server database connection string
- `BlobStorage` — Azure Blob Storage settings, including connection string, container name, account name and account key

## Testing

Run tests with:

```bash
dotnet test Restaurants.sln
```

## Deployment

The repository includes build and publish targets for local and Azure deployment.

- Build the solution: `dotnet build Restaurants.sln`
- Publish the API: `dotnet publish src/Restaurants.API/Restaurants.API.csproj -c Release -o artifacts/azure/Restaurants.API`

## Notes

- Swagger is enabled in the Development environment.
- Database seeding runs automatically on startup unless the environment is `Testing`.
- CORS is configured with an `AllowAll` policy for development convenience.
- Identity and role management are provided through ASP.NET Core Identity.

## Contact

For questions about the architecture or implementation, inspect the layered projects under `src/` and the controller implementations in `src/Restaurants.API/Controllers`.
