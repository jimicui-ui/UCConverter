# Development Guide

## Table of Contents
1. [Overview](#1-overview)
2. [Prerequisites](#2-prerequisites)
3. [Project Structure](#3-project-structure)
4. [Getting Started](#4-getting-started)
5. [Architecture Overview](#5-architecture-overview)
6. [Development Workflow](#6-development-workflow)
7. [Testing](#7-testing)
8. [Adding New Features](#8-adding-new-features)
9. [Localization](#9-localization)
10. [Code Standards](#10-code-standards)
11. [Troubleshooting](#11-troubleshooting)

---

## 1. Overview

This is an open-source Unit Converter Application built with:
- **Backend**: .NET 8 RESTful API following Clean Architecture and SOLID principles
- **Frontend**: React with TypeScript, Vite, and i18next for internationalization
- **Architecture**: 4-layer Clean Architecture (Presentation → Application → Domain → Infrastructure)
- **Testing**: xUnit with Moq, targeting ≥95% code coverage for all layers

The application uses **SI (International System of Units) base units** as the foundation for all conversions, ensuring accuracy and international compatibility.

---

## 2. Prerequisites

### Required Software

#### Backend Development
- **.NET 8 SDK** or later
  - Download from: https://dotnet.microsoft.com/download
  - Verify installation: `dotnet --version`
- **Visual Studio 2022** (recommended) or **Visual Studio Code** with C# extension
- **Git** for version control

#### Frontend Development
- **Node.js** 18.x or later
  - Download from: https://nodejs.org/
  - Verify installation: `node --version` and `npm --version`
- **npm** or **yarn** package manager

#### Optional Tools
- **Postman** or **Swagger UI** for API testing
- **Azure CLI** (if deploying to Azure)

### Development Environment Setup

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd UCConverter
   ```

2. **Verify .NET SDK**:
   ```bash
   dotnet --version  # Should show 8.0.x or later
   ```

3. **Verify Node.js**:
   ```bash
   node --version  # Should show 18.x or later
   npm --version
   ```

---

## 3. Project Structure

The solution follows a **Clean Architecture** pattern with clear separation of concerns:

```
UCConverter/
├── SolutionDotnetReact/
│   ├── src/                                    # Source code
│   │   ├── UCConverter.Api/                   # Presentation Layer
│   │   │   ├── Controllers/                    # API controllers
│   │   │   ├── Program.cs                      # Application entry point
│   │   │   └── Resources/                      # Localization resources
│   │   │
│   │   ├── UCConverter.Application/            # Application Layer
│   │   │   ├── Services/                       # Application services
│   │   │   ├── DTOs/                           # Data Transfer Objects
│   │   │   ├── Interfaces/                     # Application interfaces
│   │   │   └── Mappings/                       # DTO to Domain mappings
│   │   │
│   │   ├── UCConverter.Domain/                 # Domain Layer (Core)
│   │   │   ├── Entities/                       # Domain entities
│   │   │   ├── Interfaces/                     # Domain interfaces
│   │   │   ├── Services/                       # Domain services
│   │   │   └── Exceptions/                     # Domain exceptions
│   │   │
│   │   └── UCConverter.Infrastructure/         # Infrastructure Layer
│   │       ├── Repositories/                   # Repository implementations
│   │       └── Data/                           # Data access logic
│   │
│   ├── frontend/                               # React frontend
│   │   ├── src/
│   │   │   ├── components/                     # React components
│   │   │   ├── services/                       # API service layer
│   │   │   ├── i18n/                           # Internationalization
│   │   │   └── types/                          # TypeScript types
│   │   ├── package.json
│   │   └── vite.config.ts
│   │
│   ├── tests/                                  # Test projects
│   │   ├── UCConverter.Domain.Tests/
│   │   ├── UCConverter.Application.Tests/
│   │   ├── UCConverter.Infrastructure.Tests/
│   │   ├── UCConverter.Api.Tests/
│   │   └── UCConverter.IntegrationTests/
│   │
│   ├── UnitsSettings/                          # Unit configuration files
│   │   ├── length.json
│   │   ├── weight.json
│   │   ├── volume.json
│   │   ├── area.json
│   │   ├── temperature.json
│   │   ├── time.json
│   │   └── speed.json
│   │
│   └── UCConverter.sln                         # Solution file
│
└── docs/                                       # Documentation
    ├── Requirement.md
    ├── IMPLEMENTATION.md
    ├── VULNERABILITY_TESTING.md
    └── DEVELOPMENT_GUIDE.md
```

### Layer Dependencies

**Critical**: Dependencies flow **inward** only:
- `UCConverter.Api` → `UCConverter.Application` → `UCConverter.Domain`
- `UCConverter.Infrastructure` → `UCConverter.Domain`
- **Domain layer has NO dependencies** on other layers (pure business logic)

---

## 4. Getting Started

### 4.1 Backend Setup

1. **Navigate to solution directory**:
   ```bash
   cd SolutionDotnetReact
   ```

2. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

3. **Build the solution**:
   ```bash
   dotnet build
   ```

4. **Run the API**:
   ```bash
   cd src/UCConverter.Api
   dotnet run
   ```

   The API will start on `https://localhost:5185` (or the port configured in `launchSettings.json`).

5. **Access Swagger UI**:
   - Open browser: `https://localhost:5185/swagger`
   - Interactive API documentation and testing interface

### 4.2 Frontend Setup

1. **Navigate to frontend directory**:
   ```bash
   cd SolutionDotnetReact/frontend
   ```

2. **Install dependencies**:
   ```bash
   npm install
   ```

3. **Start development server**:
   ```bash
   npm run dev
   ```

   The frontend will start on `http://localhost:3000` (configured in `vite.config.ts`).

4. **Build for production**:
   ```bash
   npm run build
   ```

### 4.3 Running Tests

#### Run All Tests
```bash
cd SolutionDotnetReact
dotnet test
```

#### Run Tests with Coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

#### Run Specific Test Project
```bash
dotnet test tests/UCConverter.Domain.Tests/UCConverter.Domain.Tests.csproj
```

#### Generate Coverage Report
```bash
# Install ReportGenerator (if not already installed)
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generate HTML report
reportgenerator -reports:"**/coverage.opencover.xml" -targetdir:"coverage-report" -reporttypes:Html
```

---

## 5. Architecture Overview

### 5.1 Clean Architecture Layers

#### Presentation Layer (`UCConverter.Api`)
- **Responsibility**: HTTP concerns, request/response handling
- **Contains**:
  - API Controllers (`CategoriesController`, `ConvertController`)
  - Request/Response DTOs
  - Input validation
  - HTTP status code handling
  - Swagger/OpenAPI configuration
- **Dependencies**: Application layer only

#### Application Layer (`UCConverter.Application`)
- **Responsibility**: Use cases, orchestration, application logic
- **Contains**:
  - Application services (`UnitConverterService`, `LocalizationService`)
  - DTOs (Data Transfer Objects)
  - DTO to Domain entity mapping
  - Application-level validation
- **Dependencies**: Domain layer only

#### Domain Layer (`UCConverter.Domain`)
- **Responsibility**: Business logic, domain entities, core rules
- **Contains**:
  - Domain entities (`Unit`, `Category`, `ConversionResult`)
  - Domain services (`ConversionService`)
  - Domain interfaces (`IUnitRepository`, `IConversionService`)
  - Domain exceptions
- **Dependencies**: **NONE** (pure business logic)

#### Infrastructure Layer (`UCConverter.Infrastructure`)
- **Responsibility**: Data access, external services, implementations
- **Contains**:
  - Repository implementations (`JsonUnitRepository`)
  - JSON file reading logic
  - Unit configuration loading
  - Caching (in-memory)
- **Dependencies**: Domain layer only

### 5.2 SOLID Principles

The solution strictly adheres to SOLID principles:

- **Single Responsibility**: Each class has one reason to change
  - Example: `ConversionService` handles conversion logic only
- **Open/Closed**: Open for extension, closed for modification
  - Example: New units can be added via JSON files without code changes
- **Liskov Substitution**: Derived classes are substitutable
  - Example: Any `IUnitRepository` implementation can replace another
- **Interface Segregation**: Clients don't depend on unused interfaces
  - Example: Separate interfaces for different concerns
- **Dependency Inversion**: Depend on abstractions, not concretions
  - Example: Application depends on `IUnitRepository` (interface), not `JsonUnitRepository` (implementation)

### 5.3 Unit Configuration System

Units are defined in JSON files located in `UnitsSettings/` folder:

- **One JSON file per category** (e.g., `length.json`, `weight.json`)
- **Loaded at application startup** by `JsonUnitRepository`
- **Cached in memory** for fast access
- **SI base units** are used as the foundation for all conversions

**Example JSON structure** (`UnitsSettings/weight.json`):
```json
{
  "category": "weight",
  "categoryDisplayName": "Weight / Mass",
  "baseUnit": {
    "symbol": "kg",
    "name": "kilogram",
    "isBaseUnit": true,
    "isSIUnit": true,
    "unitSystem": "SI"
  },
  "units": [
    {
      "symbol": "kg",
      "name": "kilogram",
      "displayName": "Kilogram",
      "category": "weight",
      "isBaseUnit": true,
      "isSIUnit": true,
      "unitSystem": "SI",
      "conversionFactor": 1.0,
      "conversionFormula": null
    }
  ]
}
```

---

## 6. Development Workflow

### 6.1 Creating a New Feature

1. **Create a feature branch**:
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Follow the layer structure**:
   - Start from **Domain layer** (if adding new business logic)
   - Move to **Infrastructure layer** (if adding data access)
   - Then **Application layer** (for use cases)
   - Finally **Presentation layer** (for API endpoints)

3. **Write tests first** (TDD approach recommended):
   - Write unit tests for Domain logic
   - Write integration tests for API endpoints
   - Ensure ≥95% code coverage

4. **Implement the feature**:
   - Follow SOLID principles
   - Maintain clean architecture boundaries
   - Add XML documentation comments

5. **Run tests and verify coverage**:
   ```bash
   dotnet test /p:CollectCoverage=true
   ```

6. **Update Swagger documentation** (if adding API endpoints):
   - Add XML comments to controllers and DTOs
   - Swagger will auto-generate documentation

7. **Commit and push**:
   ```bash
   git add .
   git commit -m "feat: Add your feature description"
   git push origin feature/your-feature-name
   ```

### 6.2 Adding a New Unit Category

1. **Create JSON file** in `UnitsSettings/`:
   - File name: `{category}.json` (e.g., `pressure.json`)
   - Follow the JSON structure from existing files
   - Define base unit (preferably SI base or derived unit)
   - Add all units with conversion factors

2. **No code changes required**:
   - The system automatically loads all JSON files at startup
   - Categories are discovered dynamically

3. **Add localization** (if needed):
   - Add translations in `UCConverter.Application/Resources/`
   - Add frontend translations in `frontend/src/i18n/locales/`

4. **Test**:
   - Verify category appears in `/categories` endpoint
   - Test conversions for the new category
   - Add integration tests

### 6.3 Adding a New API Endpoint

1. **Define DTOs** in `UCConverter.Application/DTOs/`:
   ```csharp
   public class YourRequestDto
   {
       public string Property { get; set; }
   }
   ```

2. **Add Application Service method** in `UCConverter.Application/Services/`:
   ```csharp
   public async Task<YourResponseDto> YourMethod(YourRequestDto request)
   {
       // Application logic
   }
   ```

3. **Create Controller** in `UCConverter.Api/Controllers/`:
   ```csharp
   [ApiController]
   [Route("api/[controller]")]
   public class YourController : ControllerBase
   {
       private readonly IYourService _service;
       
       [HttpPost]
       public async Task<IActionResult> YourEndpoint([FromBody] YourRequestDto request)
       {
           var result = await _service.YourMethod(request);
           return Ok(result);
       }
   }
   ```

4. **Add XML documentation** for Swagger:
   ```csharp
   /// <summary>
   /// Description of your endpoint
   /// </summary>
   /// <param name="request">Request description</param>
   /// <returns>Response description</returns>
   [HttpPost]
   public async Task<IActionResult> YourEndpoint([FromBody] YourRequestDto request)
   ```

5. **Write integration tests**:
   - Add test in `UCConverter.IntegrationTests/`
   - Test success and error scenarios

---

## 7. Testing

### 7.1 Testing Requirements

- **Code Coverage**: ≥95% for all layers
- **Framework**: xUnit
- **Mocking**: Moq
- **Test Organization**: Separate test project for each layer
- **Naming Convention**: `MethodName_Scenario_ExpectedBehavior`

### 7.2 Unit Testing

#### Test Structure (AAA Pattern)

```csharp
[Fact]
public void Convert_ValidInput_ReturnsCorrectResult()
{
    // Arrange
    var service = new ConversionService();
    var fromUnit = new Unit { Symbol = "m", ConversionFactor = 1.0 };
    var toUnit = new Unit { Symbol = "ft", ConversionFactor = 0.3048 };
    var value = 10.0;

    // Act
    var result = service.Convert(value, fromUnit, toUnit);

    // Assert
    Assert.Equal(32.8084, result, 4);
}
```

#### Running Unit Tests

```bash
# Run all unit tests
dotnet test

# Run specific test project
dotnet test tests/UCConverter.Domain.Tests/

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### 7.3 Integration Testing

Integration tests verify end-to-end API functionality:

```csharp
[Fact]
public async Task GetCategories_ReturnsAllCategories()
{
    // Arrange
    var client = _factory.CreateClient();

    // Act
    var response = await client.GetAsync("/api/categories");

    // Assert
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var categories = JsonSerializer.Deserialize<List<CategoryDto>>(content);
    Assert.NotEmpty(categories);
}
```

#### Running Integration Tests

```bash
dotnet test tests/UCConverter.IntegrationTests/
```

### 7.4 Coverage Reports

Generate HTML coverage report:

```bash
# Install ReportGenerator (one-time)
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generate report
reportgenerator -reports:"**/coverage.opencover.xml" -targetdir:"coverage-report" -reporttypes:Html

# Open report
start coverage-report/index.html  # Windows
open coverage-report/index.html   # macOS
```

### 7.5 Test Best Practices

1. **Test Independence**: Each test should run independently
2. **Fast Execution**: Unit tests should complete in < 30 seconds total
3. **Clear Naming**: Use descriptive test method names
4. **Coverage**: Aim for ≥95% code coverage
5. **Mock External Dependencies**: Use Moq for repositories and services
6. **Test Edge Cases**: Include boundary conditions and error scenarios

---

## 8. Adding New Features

### 8.1 Adding a New Unit to Existing Category

1. **Edit JSON file** in `UnitsSettings/{category}.json`:
   ```json
   {
     "symbol": "newUnit",
     "name": "newunit",
     "displayName": "New Unit",
     "category": "length",
     "isBaseUnit": false,
     "isSIUnit": false,
     "unitSystem": "Custom",
     "conversionFactor": 0.5,
     "conversionFormula": null
   }
   ```

2. **Add localization** (if needed):
   - Backend: `UCConverter.Application/Resources/SharedResources.{locale}.resx`
   - Frontend: `frontend/src/i18n/locales/{locale}.json`

3. **Restart API** (or reload configuration if hot-reload is implemented)

4. **Test**: Verify unit appears in `/categories/{category}/units` endpoint

### 8.2 Adding Formula-Based Conversion

For non-linear conversions (e.g., temperature):

1. **Update JSON** with `conversionFormula`:
   ```json
   {
     "symbol": "°F",
     "conversionFormula": "((x - 32) * 5/9) + 273.15",
     "reverseFormula": "((x - 273.15) * 9/5) + 32"
   }
   ```

2. **Update `ConversionService`** to handle formula evaluation (if not already supported)

3. **Add unit tests** for formula-based conversions

### 8.3 Adding Frontend Feature

1. **Create component** in `frontend/src/components/`:
   ```typescript
   export const YourComponent: React.FC = () => {
     // Component logic
   };
   ```

2. **Add TypeScript types** in `frontend/src/types/`:
   ```typescript
   export interface YourType {
     property: string;
   }
   ```

3. **Add API service method** in `frontend/src/services/api.ts`:
   ```typescript
   export const yourApiMethod = async (): Promise<YourType> => {
     const response = await fetch('/api/your-endpoint');
     return response.json();
   };
   ```

4. **Add localization** in `frontend/src/i18n/locales/`:
   ```json
   {
     "yourKey": "Your translated text"
   }
   ```

5. **Test**: Run frontend and verify feature works

---

## 9. Localization

### 9.1 Backend Localization

Localization resources are in `UCConverter.Application/Resources/`:

- `SharedResources.en.resx` - English
- `SharedResources.zh.resx` - Chinese

**Adding a new translation key**:

1. Open `SharedResources.en.resx`
2. Add new entry:
   ```
   Name: ErrorMessage.InvalidUnit
   Value: Invalid unit specified
   ```

3. Add corresponding translation in `SharedResources.zh.resx`:
   ```
   Name: ErrorMessage.InvalidUnit
   Value: 指定的单位无效
   ```

4. Use in code:
   ```csharp
   var message = _localizationService.GetString("ErrorMessage.InvalidUnit");
   ```

### 9.2 Frontend Localization

Frontend uses `i18next` and `react-i18next`:

**Translation files**:
- `frontend/src/i18n/locales/en.json` - English
- `frontend/src/i18n/locales/zh.json` - Chinese

**Adding a new translation**:

1. Add to `en.json`:
   ```json
   {
     "yourKey": "Your English text"
   }
   ```

2. Add to `zh.json`:
   ```json
   {
     "yourKey": "您的中文文本"
   }
   ```

3. Use in component:
   ```typescript
   import { useTranslation } from 'react-i18next';
   
   const { t } = useTranslation();
   return <div>{t('yourKey')}</div>;
   ```

### 9.3 API Localization

API supports localization via:
- **Query parameter**: `?locale=en-US` or `?locale=zh-CN`
- **HTTP Header**: `Accept-Language: en-US` or `Accept-Language: zh-CN`

---

## 10. Code Standards

### 10.1 C# Coding Standards

- **Naming Conventions**:
  - Classes: `PascalCase` (e.g., `UnitConverterService`)
  - Methods: `PascalCase` (e.g., `ConvertUnit`)
  - Properties: `PascalCase` (e.g., `Symbol`)
  - Private fields: `_camelCase` (e.g., `_repository`)
  - Local variables: `camelCase` (e.g., `resultValue`)

- **File Organization**:
  - One class per file
  - File name matches class name

- **XML Documentation**:
  ```csharp
  /// <summary>
  /// Converts a value from one unit to another.
  /// </summary>
  /// <param name="value">The value to convert</param>
  /// <param name="fromUnit">Source unit</param>
  /// <param name="toUnit">Target unit</param>
  /// <returns>Converted value</returns>
  public double Convert(double value, Unit fromUnit, Unit toUnit)
  ```

- **SOLID Principles**: Always follow SOLID principles
- **Dependency Injection**: Use constructor injection
- **Async/Await**: Use async methods for I/O operations

### 10.2 TypeScript/React Coding Standards

- **Naming Conventions**:
  - Components: `PascalCase` (e.g., `UnitConverter`)
  - Functions: `camelCase` (e.g., `handleConvert`)
  - Constants: `UPPER_SNAKE_CASE` (e.g., `API_BASE_URL`)
  - Types/Interfaces: `PascalCase` (e.g., `ConvertRequest`)

- **Component Structure**:
  ```typescript
  import React from 'react';
  
  interface Props {
    // Props definition
  }
  
  export const Component: React.FC<Props> = ({ prop }) => {
    // Component logic
    return <div>Content</div>;
  };
  ```

- **Type Safety**: Always use TypeScript types, avoid `any`
- **Hooks**: Use React hooks for state management
- **Error Handling**: Use ErrorBoundary for error handling

### 10.3 Git Commit Messages

Follow [Conventional Commits](https://www.conventionalcommits.org/):

- `feat:` - New feature
- `fix:` - Bug fix
- `docs:` - Documentation changes
- `test:` - Test additions/changes
- `refactor:` - Code refactoring
- `chore:` - Build process or auxiliary tool changes

**Example**:
```
feat: Add pressure unit category support
fix: Correct temperature conversion formula
docs: Update API documentation
```

---

## 11. Troubleshooting

### 11.1 Backend Issues

#### API Not Starting
- **Check .NET SDK version**: `dotnet --version` (should be 8.0+)
- **Restore packages**: `dotnet restore`
- **Check `UnitsSettings` folder**: Ensure JSON files exist at solution root
- **Check port conflicts**: Verify port 5185 is available

#### Units Not Loading
- **Verify `UnitsSettings` path**: Check `Program.cs` for correct path resolution
- **Check JSON file format**: Validate JSON syntax
- **Check logs**: Look for initialization errors in console output

#### Swagger Not Accessible
- **Verify Swagger configuration**: Check `Program.cs` for Swagger setup
- **Check URL**: Should be `https://localhost:5185/swagger`
- **HTTPS certificate**: May need to trust development certificate

### 11.2 Frontend Issues

#### Dependencies Not Installing
- **Clear npm cache**: `npm cache clean --force`
- **Delete node_modules**: `rm -rf node_modules` (or `Remove-Item -Recurse -Force node_modules` on Windows)
- **Reinstall**: `npm install`

#### API Calls Failing
- **Check API is running**: Verify backend is running on correct port
- **Check CORS**: Verify CORS is configured in `Program.cs`
- **Check proxy**: Verify `vite.config.ts` proxy configuration
- **Check network tab**: Inspect browser DevTools for errors

#### Build Errors
- **TypeScript errors**: Run `npm run build` to see detailed errors
- **Linting errors**: Run `npm run lint` to check ESLint issues

### 11.3 Test Issues

#### Tests Not Running
- **Restore packages**: `dotnet restore`
- **Build solution**: `dotnet build`
- **Check test project references**: Verify test projects reference correct source projects

#### Coverage Not Generated
- **Check Coverlet package**: Verify `coverlet.msbuild` is referenced
- **Check output format**: Use `/p:CoverletOutputFormat=opencover`
- **Check test execution**: Ensure tests actually run (not skipped)

#### Integration Tests Failing
- **Check test server**: Verify `WebApplicationFactory` is configured correctly
- **Check test data**: Ensure test JSON files exist
- **Check database/state**: Ensure tests don't depend on external state

### 11.4 Common Solutions

#### Clear All Build Artifacts
```bash
# Backend
dotnet clean
dotnet restore
dotnet build

# Frontend
rm -rf node_modules
rm -rf dist
npm install
```

#### Reset Development Environment
```bash
# Stop all running processes
# Clear all build artifacts (see above)
# Restart IDE
# Rebuild solution
```

---

## Additional Resources

- **Requirements Document**: See `docs/Requirement.md` for detailed requirements
- **Implementation Details**: See `docs/IMPLEMENTATION.md` for implementation specifics
- **Vulnerability Testing**: See `docs/VULNERABILITY_TESTING.md` for security testing guidelines
- **Swagger UI**: Access at `https://localhost:5185/swagger` when API is running
- **.NET Documentation**: https://docs.microsoft.com/dotnet/
- **React Documentation**: https://react.dev/
- **TypeScript Documentation**: https://www.typescriptlang.org/docs/

---

## Getting Help

If you encounter issues not covered in this guide:

1. **Check existing issues**: Search GitHub issues
2. **Review logs**: Check application logs for error messages
3. **Verify requirements**: Ensure all prerequisites are met
4. **Test in isolation**: Create minimal reproduction case
5. **Ask for help**: Create a detailed issue with:
   - Error messages
   - Steps to reproduce
   - Environment details (OS, .NET version, Node version)
   - Relevant code snippets

---

**Last Updated**: Based on current solution structure and requirements document.

