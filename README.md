# Patient Management API

## Overview
This API provides a complete solution for managing patient records with CRUD operations and advanced search capabilities. The system follows RESTful principles and includes FHIR-compatible search parameters for birthdate queries.

## API Endpoints

### Patient Operations
- `GET /api/patients` - Retrieve all patients
- `GET /api/patients/{id}` - Get specific patient by ID
- `POST /api/patients` - Create new patient
- `PUT /api/patients/{id}` - Update existing patient
- `DELETE /api/patients/{id}` - Delete patient

### Advanced Search
- `GET /api/patients/search?date={fhirDate}` - Search patients by birthdate using FHIR parameters:
  - `eq2013-01-14` (equal)
  - `ne2013-01-14` (not equal)
  - `lt2013-01-14` (less than)
  - `gt2013-01-14` (greater than)
  - `le2013-01-14` (less or equal)
  - `ge2013-01-14` (greater or equal)
  - `sa2013-01-14` (starts after)
  - `eb2013-01-14` (ends before)
  - `ap2013-01-14` (approximately)

## Development Setup

### Prerequisites
- Docker Desktop
- .NET 8 SDK
- SQL Server (included in Docker setup)

### Running with Docker Compose

1. **Build and run containers**:
   ```bash
   docker-compose up -d --build
   ```

2. **Verify services are running**:
   ```bash
   docker-compose ps
   ```

3. **Access the API**:
   - Swagger UI: http://localhost:7272/swagger/index.html
   - API Base URL: http://localhost:7272/api/patients

4. **Stop services**:
   ```bash
   docker-compose down
   ```

### Configuration
The system uses these environment variables (configured in docker-compose.yml):
- `ConnectionStrings__DefaultConnection`: Database connection string
- `ASPNETCORE_ENVIRONMENT`: Runtime environment (Development/Production)

## Testing the API

A Postman collection (`Patient API Demo.postman_collection.json`) is included in the project root with pre-configured requests for all endpoints. Import this into Postman for quick testing.

## Database Migrations
The system automatically applies EF Core migrations on startup when running in Development mode.

## Health Checks
- Endpoint: `GET /health`
- Checks: Database connectivity

## Project Structure
```
/Patient.API
  ├── Controllers/       # API endpoints
  ├── Core/             # Business logic and DTOs
  ├── Infrastructure/   # Data access layer
  ├── Migrations/       # Database migrations
  ├── Dockerfile        # Container configuration
  ├── docker-compose.yml # Service orchestration
  ├── appsettings.json  # Configuration
  └── Patient API Demo.postman_collection.json # Test collection
```
