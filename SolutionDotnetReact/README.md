# UCConverter - Unit Converter Solution

This solution implements a Unit Converter Application following Clean Architecture principles and SOLID design patterns.

## Solution Structure

```
SolutionDotnetReact/
├── src/
│   ├── UCConverter.Domain/              # Domain Layer (No dependencies)
│   ├── UCConverter.Application/         # Application Layer (References Domain)
│   ├── UCConverter.Infrastructure/       # Infrastructure Layer (References Domain)
│   └── UCConverter.Api/                 # Presentation Layer (References Application)
├── tests/
│   ├── UCConverter.Domain.Tests/         # Unit tests for Domain
│   ├── UCConverter.Application.Tests/   # Unit tests for Application
│   ├── UCConverter.Infrastructure.Tests/ # Unit tests for Infrastructure
│   ├── UCConverter.Api.Tests/            # Unit tests for API
│   └── UCConverter.IntegrationTests/    # Integration tests for all endpoints
├── UnitsSettings/                       # Unit configuration JSON files
│   ├── length.json
│   ├── weight.json
│   ├── temperature.json
│   ├── volume.json
│   └── ... (one file per category)
└── UCConverter.sln                      # Solution file
```

## Architecture

This solution follows **Clean Architecture** with 4 layers:

1. **Domain Layer** - Business entities, domain logic, interfaces (no dependencies)
2. **Application Layer** - Use cases, services, DTOs (depends on Domain)
3. **Infrastructure Layer** - Data access, external services (depends on Domain)
4. **Presentation Layer** - API controllers, endpoints (depends on Application)

## Project Dependencies

- `UCConverter.Api` → `UCConverter.Application` → `UCConverter.Domain`
- `UCConverter.Infrastructure` → `UCConverter.Domain`
- Domain layer has **NO project references** (pure business logic)

## Requirements Compliance

✅ **SOLID Principles** - All layers follow SOLID principles  
✅ **Clean Architecture** - 4-layer architecture with proper dependency flow  
✅ **Separate Projects** - Each layer is a separate .NET project  
✅ **SI Units Support** - Uses SI base units as default  
✅ **Unit Configuration** - JSON files in UnitsSettings folder  
✅ **Test Projects** - Separate test projects for each layer  
✅ **100% Coverage** - Unit tests and integration tests required  
✅ **.NET 8** - All projects target .NET 8.0  

## Building the Solution

```bash
dotnet build
```

## Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Next Steps

1. Implement Domain entities (Unit, Category, etc.)
2. Implement Domain interfaces (IUnitRepository, etc.)
3. Implement Infrastructure repositories (JSON file loading)
4. Implement Application services (conversion logic)
5. Implement API controllers/endpoints
6. Write unit tests for each layer
7. Write integration tests for all endpoints

## Unit Configuration Files

Unit definitions are stored in JSON files in the `UnitsSettings` folder. Each category has its own file (e.g., `weight.json`, `length.json`). These files are loaded at application startup by the Infrastructure layer.

See `docs/Requirement.md` for complete requirements and specifications.

