# Final Requirements Document – Open-Source Unit Converter App

## Table of Contents
1. [Project Overview](#1-project-overview)
2. [Functional Requirements](#2-functional-requirements)
   - [2.1 Core Conversion Features](#21-core-conversion-features)
   - [2.2 Localization](#22-localization-internationalization)
   - [2.3 Backend (API) Requirements](#23-backend-api-requirements)
     - [Architecture Principles (SOLID & Clean Architecture)](#architecture-principles)
   - [2.4 Frontend (React) Requirements](#24-frontend-react-requirements)
   - [2.5 International System of Units (SI) Support](#25-international-system-of-units-si-support)
   - [2.6 Data, Storage & Configuration](#26-data-storage--configuration)
3. [Non-Functional Requirements](#3-non-functional-requirements)
   - [3.1 Performance](#31-performance)
   - [3.2 Scalability](#32-scalability)
   - [3.3 Reliability & Availability](#33-reliability--availability)
   - [3.4 Security](#34-security)
   - [3.5 Usability & Accessibility](#35-usability--accessibility)
   - [3.6 Testing Requirements](#36-testing-requirements)
4. [Architecture Overview](#4-architecture-overview)
   - [4.1 System Components](#41-system-components)
   - [4.2 Backend Layered Architecture](#42-backend-layered-architecture)
   - [4.3 Deployment](#43-deployment)
5. [Unit Categories to Support](#5-unit-categories-to-support)
6. [API Contract Proposal](#6-api-contract-proposal)
7. [Roadmap](#7-roadmap)
8. [Cost Considerations](#8-cost-considerations-azure)

---

## 1. Project Overview

An open-source, multi-language Unit Converter Application that provides accurate conversion across various unit categories (length, mass, volume, etc.). The system is built on the **International System of Units (SI)** standard, using SI base units as the foundation for all conversions to ensure accuracy and international compatibility.

### System Components

- **Backend**: .NET 8 / RESTful API hosted on Azure App Service or Azure Functions
- **Frontend**: React web application
- **Internationalization**: Full UI + API localization
- **Open-source**: Public GitHub repo with contribution guidelines

### Goals

The app aims to be **simple**, **fast**, **extensible**, and **low-cost**.

---

## 2. Functional Requirements

### 2.1 Core Conversion Features

- Convert a numeric value from a source unit to a target unit within the same category
- Support both:
  - Simple linear conversions (e.g., length: meters to feet)
  - Formula-based conversions (e.g., temperature: Celsius to Fahrenheit)
- Allow batch conversions: one input → many target units
- **International System of Units (SI) Support**:
  - Use SI base units as the default/base units for internal calculations
  - Mark SI units clearly in the API and UI
  - Support both SI and non-SI units (e.g., imperial, US customary)
- Provide metadata endpoints for:
  - List of all supported categories
  - List of units per category
  - Unit symbols, names, and localized display text
  - **Unit metadata including**:
    - Whether the unit is an SI base unit (default)
    - Whether the unit is an SI derived unit
    - Unit system classification (SI, Imperial, US Customary, etc.)

### 2.2 Localization (Internationalization)

- Support multiple languages (starting with English and Chinese, more languages to be added later)
- API should offer language options via:
  - HTTP header (e.g., `Accept-Language`)
  - Query parameter (e.g., `?locale=en-US`)
- React frontend should support dynamic language switching
- Localized content for:
  - Unit categories
  - Unit names
  - Error messages
  - UI labels

### 2.3 Backend (API) Requirements

#### Architecture Principles

The backend API implementation must follow **SOLID principles** and **Clean Architecture** (Layered Architecture) pattern:

##### SOLID Principles
- **S - Single Responsibility Principle**: Each class/component should have one reason to change
- **O - Open/Closed Principle**: Open for extension, closed for modification
- **L - Liskov Substitution Principle**: Derived classes must be substitutable for their base classes
- **I - Interface Segregation Principle**: Clients should not depend on interfaces they don't use
- **D - Dependency Inversion Principle**: Depend on abstractions, not concretions

##### Layered Architecture (Clean Architecture)
The solution must follow a **4-layer architecture** pattern where **each layer is implemented as a separate .NET project** (.csproj file):

```
┌─────────────────────────────────────┐
│     Presentation Layer              │  Controllers, API endpoints, DTOs
│  (Separate Project: *.Api)         │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│     Application Layer               │  Use cases, services, application logic
│  (Separate Project: *.Application) │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│     Domain Layer                     │  Business entities, domain logic, interfaces
│  (Separate Project: *.Domain)       │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│     Infrastructure Layer             │  Data access, external services, implementations
│  (Separate Project: *.Infrastructure)│
└─────────────────────────────────────┘
```

**Project Separation Requirements**:
- **Each layer MUST be a separate .NET project** (.csproj)
- This enforces proper dependency management and separation of concerns
- Prevents accidental cross-layer dependencies
- Enables independent testing and deployment of layers

**Layer Responsibilities**:

1. **Presentation Layer**:
   - API controllers or minimal API endpoints
   - Request/Response DTOs (Data Transfer Objects)
   - Input validation
   - HTTP status code handling
   - Authentication/Authorization (if needed)

2. **Application Layer**:
   - Application services (use cases)
   - Orchestration of domain logic
   - Transaction management
   - Mapping between domain models and DTOs
   - Application-specific validation

3. **Domain Layer**:
   - Domain entities and value objects
   - Business rules and domain logic
   - Domain interfaces (repositories, services)
   - Domain exceptions
   - **No dependencies on other layers**

4. **Infrastructure Layer**:
   - Repository implementations
   - Data access (JSON file reading from `UnitsSettings` folder, Azure Table Storage, etc.)
   - **Unit configuration loading**: Load all JSON files from `UnitsSettings` folder at startup
   - External service clients
   - Caching implementations (in-memory cache for loaded units)
   - Logging implementations

**Dependency Rules**:
- Dependencies flow **inward**: Presentation → Application → Domain ← Infrastructure
- Domain layer has **no dependencies** on other layers
- Infrastructure implements interfaces defined in Domain layer
- Use **Dependency Injection** to manage dependencies

#### Technical Architecture
- RESTful architecture using .NET 8 (minimal APIs or controllers)
- Follow Clean Architecture / Onion Architecture pattern
- Implement Dependency Injection using .NET built-in DI container

#### Endpoints
- `GET /categories` - Retrieve all supported unit categories
- `GET /categories/{name}/units` - Get units for a specific category (includes unit metadata)
- `POST /convert` - Perform unit conversion
- `GET /units/{unitSymbol}` - Get detailed information about a specific unit (optional)

#### API Documentation (Swagger/OpenAPI)

The API must include comprehensive interactive API documentation using **Swagger/OpenAPI**.

**Swagger/OpenAPI Requirements**:
- **Swagger UI Integration**: Implement Swagger UI for interactive API documentation and testing
- **OpenAPI Specification**: Generate and maintain OpenAPI 3.0 specification for all API endpoints
- **Documentation Coverage**: All endpoints must be fully documented with:
  - Endpoint descriptions and purpose
  - Request/response schemas with detailed property descriptions
  - Parameter descriptions (path, query, header parameters)
  - Request body examples
  - Response examples (success and error scenarios)
  - HTTP status codes and their meanings
  - Authentication requirements (if applicable)
- **Interactive Testing**: Swagger UI must allow developers to test API endpoints directly from the documentation interface
- **Localization Support**: API documentation should reflect localized error messages and descriptions when applicable

**Implementation Details**:
- Use **Swashbuckle.AspNetCore** (or similar .NET library) for Swagger/OpenAPI integration
- Configure Swagger UI to be available in development and optionally in production (with appropriate security)
- Generate OpenAPI JSON/YAML specification that can be exported and used by API clients
- Include XML comments from code to automatically populate Swagger documentation
- Document all DTOs, models, and enums with XML documentation comments
- Provide example values for all request/response models

**Swagger Endpoints**:
- `GET /swagger` - Swagger UI interface (development/staging environments)
- `GET /swagger/v1/swagger.json` - OpenAPI JSON specification
- `GET /swagger/v1/swagger.yaml` - OpenAPI YAML specification (optional)

**Documentation Standards**:
- Use clear, concise descriptions for all endpoints and parameters
- Include practical examples for common use cases
- Document error responses with error codes and messages
- Specify data types, formats, and constraints (e.g., required fields, min/max values)
- Include unit metadata information in endpoint documentation

#### Technical Implementation

**Architecture Requirements**:
- Implement using **Clean Architecture** (4-layer pattern: Presentation → Application → Domain → Infrastructure)
- Follow **SOLID principles** throughout the codebase
- Use **Dependency Injection** for all dependencies
- Domain layer must have **zero dependencies** on other layers

**Unit Conversion System**:
- Maintain internal base-unit system using **SI (International System of Units) base units**:
  - **Length**: meter (m) - SI base unit
  - **Mass**: kilogram (kg) - SI base unit
  - **Time**: second (s) - SI base unit
  - **Temperature**: kelvin (K) - SI base unit
  - **Volume**: cubic meter (m³) - SI derived unit
  - **Area**: square meter (m²) - SI derived unit
  - **Speed**: meter per second (m/s) - SI derived unit
- Store units and conversion factors in configuration (JSON file or database)
- Each unit definition must include:
  - `isBaseUnit`: boolean flag indicating if it's the default/base unit for the category
  - `isSIUnit`: boolean flag indicating if it's an SI unit
  - `unitSystem`: string (e.g., "SI", "Imperial", "US Customary")
  - Conversion factor or formula to the base unit
- For non-linear conversions (e.g., °C ↔ °F), support formula evaluation

**Code Organization**:
- Domain entities and business logic in Domain layer
- Conversion algorithms as domain services
- Data access abstracted through repository pattern (defined in Domain, implemented in Infrastructure)
- Application services orchestrate domain logic
- Controllers/Endpoints only handle HTTP concerns

**Unit Configuration Loading**:
- Unit JSON files stored in `UnitsSettings` folder at solution root
- One JSON file per category (e.g., `weight.json`, `length.json`)
- Files must be **loaded at application startup** by Infrastructure layer
- Load all category files during `Program.cs` initialization or service registration
- Cache loaded units in memory for fast access throughout application lifetime
- Implement error handling for missing or malformed JSON files
- Log warnings for failed file loads but allow application to continue (graceful degradation)

#### Validation & Error Handling
- Validate:
  - Numeric input format
  - Unsupported unit combinations
  - Cross-category conversion attempts (should be rejected)
- Provide meaningful, localized error responses

### 2.4 Frontend (React) Requirements

#### User Interface Components
- **Selection UI**:
  - Choose category
  - Choose source unit
  - Choose target unit
  - Enter value
  - Display result

#### Features to Support
- Dark mode (optional, future enhancement)
- **Responsive Page Design**: Full support for mobile/phone and desktop devices
- Multi-language toggle

#### Responsive Page Design Requirements

The application must provide a fully responsive user interface that adapts seamlessly across different device types and screen sizes.

**Device Support**:
- **Mobile/Phone**: Support for smartphones (iOS and Android) with screen widths from 320px to 767px
- **Tablet**: Support for tablet devices with screen widths from 768px to 1023px
- **Desktop**: Support for desktop and laptop screens with widths from 1024px and above
- **Large Desktop**: Optimized for large desktop monitors (1920px and above)

**Responsive Design Principles**:
- **Mobile-First Approach**: Design and develop starting from mobile screens, then enhance for larger screens
- **Flexible Layouts**: Use CSS Grid and Flexbox for fluid, adaptive layouts
- **Responsive Typography**: Font sizes should scale appropriately across breakpoints
- **Touch-Friendly Interface**: 
  - Minimum touch target size of 44x44px for mobile devices
  - Adequate spacing between interactive elements
  - Swipe gestures where appropriate
- **Adaptive Navigation**: 
  - Mobile: Collapsible menu (hamburger menu) or bottom navigation
  - Desktop: Full horizontal navigation menu
- **Content Prioritization**: 
  - Show most important content first on mobile
  - Progressive disclosure for secondary features
- **Form Optimization**:
  - Full-width inputs on mobile for easier data entry
  - Appropriate input types (number, tel, etc.) to trigger correct mobile keyboards
  - Large, easily tappable buttons on mobile

**Breakpoint Strategy**:
- **Mobile**: < 768px
- **Tablet**: 768px - 1023px
- **Desktop**: ≥ 1024px
- **Large Desktop**: ≥ 1920px

**Layout Adaptations**:
- **Mobile**: 
  - Single-column layout for conversion inputs
  - Stacked form elements (category, source unit, target unit, value)
  - Full-width buttons
  - Compact result display
- **Tablet**: 
  - Two-column layout where appropriate
  - Side-by-side unit selectors
  - Optimized spacing and padding
- **Desktop**: 
  - Multi-column layouts for better space utilization
  - Horizontal form layouts
  - Enhanced spacing and visual hierarchy
  - Optional sidebar for favorites/history

**Performance Considerations**:
- Optimize images and assets for mobile networks
- Lazy loading for non-critical content
- Minimize JavaScript bundle size for faster mobile load times
- Use CSS media queries for responsive styling (avoid JavaScript-based layout changes where possible)

**Testing Requirements**:
- Test on real devices (iOS and Android) or device emulators
- Test across multiple screen sizes and orientations (portrait/landscape)
- Verify touch interactions work correctly on mobile devices
- Ensure text remains readable at all screen sizes
- Validate that all features are accessible on mobile devices

#### Additional Features
- Display conversion formula (optional)
- **Unit Information Display**:
  - Visual indicator (⭐) for default/base units
  - Badge or label showing SI units vs. non-SI units
  - Unit system classification (SI, Imperial, US Customary)
- Saved "Favorites" conversions (stored in local storage)
- "Recent history" of conversions
- Display precise results with controlled rounding
- Option to filter units by system (show only SI units, only Imperial, etc.)

### 2.5 International System of Units (SI) Support

#### SI Base Units (Default Units)
The system uses **SI base units** as the internal standard for all conversions:

- **Length**: meter (m) - SI base unit
- **Mass**: kilogram (kg) - SI base unit  
- **Time**: second (s) - SI base unit
- **Temperature**: kelvin (K) - SI base unit
- **Electric Current**: ampere (A) - SI base unit (Phase 2+)
- **Amount of Substance**: mole (mol) - SI base unit (Phase 2+)
- **Luminous Intensity**: candela (cd) - SI base unit (Phase 2+)

#### SI Derived Units
Common SI derived units used as defaults:

- **Volume**: cubic meter (m³)
- **Area**: square meter (m²)
- **Speed**: meter per second (m/s)
- **Force**: newton (N) = kg·m/s² (Phase 2+)
- **Pressure**: pascal (Pa) = N/m² (Phase 2+)
- **Energy**: joule (J) = N·m (Phase 2+)
- **Power**: watt (W) = J/s (Phase 2+)

#### Unit Classification
Each unit must be classified with:
- **isBaseUnit**: `true` if it's the default/base unit for its category
- **isSIUnit**: `true` if it's an official SI unit (base or derived)
- **unitSystem**: Classification (e.g., "SI", "Imperial", "US Customary", "Metric Non-SI")

#### Benefits
- **Accuracy**: SI units provide standardized, internationally recognized measurements
- **Consistency**: All conversions go through SI base units, ensuring accuracy
- **Extensibility**: Easy to add new units by converting to/from SI base units
- **Standards Compliance**: Follows international measurement standards

### 2.6 Data, Storage & Configuration

#### Unit Configuration File Structure

Unit definitions must be stored in **separate JSON files** organized by category in a dedicated folder structure:

**Folder Structure**:
```
UCSolution/
└── UnitsSettings/
    ├── length.json
    ├── weight.json          (or mass.json)
    ├── volume.json
    ├── area.json
    ├── temperature.json
    ├── time.json
    ├── speed.json
    └── ... (one file per category)
```

**File Naming Convention**:
- Each category has its own JSON file
- File name matches the category name (lowercase, singular)
- Example: `weight.json` for weight/mass category, `length.json` for length category

**File Location**:
- Files should be placed in the `UnitsSettings` folder at the solution root
- Files are embedded as content in the Infrastructure project
- Files are copied to output directory during build

**Application Startup Loading**:
- All unit configuration files must be **loaded at application startup**
- Load all JSON files from `UnitsSettings` folder during application initialization
- Cache loaded units in memory for fast access
- If a file fails to load, log error but allow application to start (graceful degradation)

**JSON File Format** (Example: `UnitsSettings/weight.json`):
```json
{
  "category": "weight",
  "categoryDisplayName": "Weight / Mass",
  "baseUnit": {
    "symbol": "kg",
    "name": "kilogram",
    "displayName": "Kilogram",
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
    },
    {
      "symbol": "g",
      "name": "gram",
      "displayName": "Gram",
      "category": "weight",
      "isBaseUnit": false,
      "isSIUnit": true,
      "unitSystem": "SI",
      "conversionFactor": 0.001,
      "conversionFormula": null
    },
    {
      "symbol": "lb",
      "name": "pound",
      "displayName": "Pound",
      "category": "weight",
      "isBaseUnit": false,
      "isSIUnit": false,
      "unitSystem": "Imperial",
      "conversionFactor": 0.453592,
      "conversionFormula": null
    }
  ]
}
```

**Storage Options**:
- **Primary**: JSON files in `UnitsSettings` folder (for MVP)
- **Future**: Azure Table Storage or Blob Storage (for cloud deployment)
- Cache static metadata (categories/units) in memory to reduce I/O operations
- **Note**: No heavy database requirements for MVP

**Implementation Requirements**:
- Infrastructure layer must implement `IUnitRepository` to read from JSON files
- Repository should load all files at startup and cache in memory
- Support lazy loading as fallback if startup loading fails
- Provide error handling for malformed JSON files

---

## 3. Non-Functional Requirements

### 3.1 Performance

- Conversion API must respond within **<50ms** under normal load (very light computation)
- Minimal latency for metadata endpoints

### 3.2 Scalability

- Backend scalable via:
  - Azure App Service Plan, or
  - Azure Functions Consumption Plan
- Caching (in-memory or Azure Cache) for frequently requested metadata

### 3.3 Reliability & Availability

- Use Azure's SLA-backed services
- Implement graceful fallback on API errors

### 3.4 Security

- Input sanitization for all user inputs
- Rate limiting for public API (Azure API Management in future)
- Optional API key authentication if the service becomes public with high usage

### 3.5 Usability & Accessibility

- Clear, intuitive interface
- **Responsive design**: Fully functional and optimized for mobile, tablet, and desktop devices
- Keyboard navigation support
- High contrast options for better visibility
- Touch-friendly interface for mobile devices (minimum 44x44px touch targets)

### 3.6 Testing Requirements

#### Unit Testing

- **Code Coverage Requirement**: **Over 95% code coverage** for all layers
  - Domain Layer: ≥95% coverage of all business logic, entities, and value objects
  - Application Layer: ≥95% coverage of all services, use cases, and mappers
  - Infrastructure Layer: ≥95% coverage of all repository implementations and data access
  - Presentation Layer: ≥95% coverage of all controllers/endpoints, DTOs, and validators

- **Testing Framework**: Use xUnit, NUnit, or MSTest
- **Mocking Framework**: Use Moq, NSubstitute, or similar for dependencies
- **Test Organization**:
  - Separate test projects for each layer
  - Test project naming: `{ProjectName}.Tests` (e.g., `UCConverter.Domain.Tests`)
  - Follow Arrange-Act-Assert (AAA) pattern
  - Use descriptive test method names: `MethodName_Scenario_ExpectedBehavior`

- **Test Categories**:
  - Unit tests for business logic
  - Unit tests for conversion algorithms
  - Unit tests for validation logic
  - Unit tests for mapping logic
  - Unit tests for error handling

- **Coverage Tools**: Use tools like:
  - Coverlet for code coverage collection
  - ReportGenerator for coverage reports
  - Integrate with CI/CD pipeline to enforce coverage requirements

#### Integration Testing

- **API Endpoint Coverage Requirement**: **Over 95% coverage of all API endpoints**
  - All endpoints must have integration tests
  - Test all HTTP methods (GET, POST, etc.)
  - Test all success scenarios
  - Test all error scenarios (400, 404, 500, etc.)
  - Test edge cases and boundary conditions

- **Endpoints to Cover**:
  - `GET /categories` - Retrieve all categories
  - `GET /categories/{name}/units` - Get units for a category
  - `POST /convert` - Perform unit conversion
  - `GET /units/{unitSymbol}` - Get unit details (if implemented)
  - All error endpoints and status codes

- **Integration Test Requirements**:
  - Use in-memory test server or test containers
  - Test with real JSON configuration files (test data)
  - Test end-to-end flow: Request → Controller → Application → Domain → Infrastructure
  - Verify response format, status codes, and data correctness
  - Test localization scenarios (different locales)
  - Test validation scenarios (invalid inputs)

- **Test Data**:
  - Use dedicated test JSON files in test project
  - Ensure test data doesn't affect production data
  - Test with various unit combinations

- **Performance Testing** (Optional for MVP):
  - Load testing for high-traffic scenarios
  - Response time validation

#### Test Execution

- **Continuous Integration**: All tests must pass in CI/CD pipeline
- **Pre-commit**: Run unit tests before code commit (optional but recommended)
- **Coverage Reports**: Generate and publish coverage reports in CI/CD
- **Coverage Gates**: Fail build if coverage drops below 95%
- **Test Execution Time**: Unit tests should complete in < 30 seconds
- **Integration tests**: Should complete in < 2 minutes

#### Test Quality Standards

- Tests must be:
  - **Independent**: Each test should run independently without dependencies
  - **Repeatable**: Tests should produce same results every time
  - **Fast**: Unit tests should execute quickly
  - **Maintainable**: Tests should be easy to understand and update
  - **Comprehensive**: Cover happy paths, error paths, and edge cases

---

## 4. Architecture Overview

### 4.1 System Components

```
┌─────────────┐
│   Frontend  │  React app, calls backend via HTTPS
│   (React)   │
└──────┬──────┘
       │
       │ HTTPS
       │
┌──────▼──────┐
│   Backend   │  .NET 8 API hosted on Azure
│  (.NET 8)   │  (Layered Architecture)
└──────┬──────┘
       │
       │
┌──────▼──────┐
│   Storage   │  JSON config or Azure Table Storage
└─────────────┘
```

### 4.2 Backend Layered Architecture

The backend follows a **Clean Architecture** pattern with four distinct layers:

```
┌─────────────────────────────────────────────┐
│         Presentation Layer                  │
│  • API Controllers / Minimal APIs          │
│  • DTOs (Request/Response models)           │
│  • Input validation                         │
│  • HTTP status handling                     │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│         Application Layer                    │
│  • Application Services                      │
│  • Use Cases / Command Handlers             │
│  • DTO to Domain mapping                     │
│  • Application-level validation              │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│         Domain Layer                         │
│  • Domain Entities (Unit, Category, etc.)    │
│  • Business Logic (Conversion algorithms)    │
│  • Domain Interfaces (IUnitRepository, etc.) │
│  • Domain Exceptions                         │
│  • Value Objects                             │
└──────────────────┬──────────────────────────┘
                   │
┌──────────────────▼──────────────────────────┐
│         Infrastructure Layer                 │
│  • Repository Implementations                │
│  • Data Access (JSON, Azure Table Storage)   │
│  • Caching (In-memory, Azure Cache)          │
│  • Logging                                   │
│  • External Service Clients                  │
└─────────────────────────────────────────────┘
```

#### Project Structure Example

**Important**: Each layer must be implemented as a **separate .NET project** (.csproj) to enforce proper separation of concerns and dependency management.

```
UCSolution/
├── src/
│   ├── UCConverter.Api/                    # Presentation Layer (Separate Project)
│   │   ├── Controllers/
│   │   ├── DTOs/
│   │   ├── Program.cs
│   │   └── UCConverter.Api.csproj          # References: Application
│   │
│   ├── UCConverter.Application/            # Application Layer (Separate Project)
│   │   ├── Services/
│   │   ├── Mappings/
│   │   ├── Validators/
│   │   └── UCConverter.Application.csproj  # References: Domain
│   │
│   ├── UCConverter.Domain/                 # Domain Layer (Separate Project)
│   │   ├── Entities/
│   │   ├── Interfaces/
│   │   ├── ValueObjects/
│   │   ├── Exceptions/
│   │   └── UCConverter.Domain.csproj       # No references (Core layer)
│   │
│   └── UCConverter.Infrastructure/         # Infrastructure Layer (Separate Project)
│       ├── Repositories/
│       ├── Data/
│       ├── Caching/
│       └── UCConverter.Infrastructure.csproj # References: Domain
│
├── UnitsSettings/                          # Unit Configuration Files
│   ├── length.json
│   ├── weight.json
│   ├── volume.json
│   ├── area.json
│   ├── temperature.json
│   ├── time.json
│   ├── speed.json
│   └── ... (one JSON file per category)
│
└── tests/
    ├── UCConverter.Domain.Tests/          # Unit tests for Domain layer (≥95% coverage required)
    ├── UCConverter.Application.Tests/     # Unit tests for Application layer (≥95% coverage required)
    ├── UCConverter.Infrastructure.Tests/  # Unit tests for Infrastructure layer (≥95% coverage required)
    ├── UCConverter.Api.Tests/             # Unit tests for Presentation layer (≥95% coverage required)
    └── UCConverter.IntegrationTests/      # Integration tests for all API endpoints (≥95% endpoint coverage required)
```

**Project Dependencies**:
- `UCConverter.Api` → `UCConverter.Application` → `UCConverter.Domain`
- `UCConverter.Application` → `UCConverter.Domain`
- `UCConverter.Infrastructure` → `UCConverter.Domain`
- **Domain layer has NO project references** (pure business logic)

**Test Project Requirements**:
- Each layer must have a corresponding unit test project
- **Over 95% code coverage** required for all unit test projects
- **Over 95% API endpoint coverage** required for integration tests
- All test projects must pass in CI/CD pipeline
- Coverage reports must be generated and validated

**UnitsSettings Folder**:
- Located at solution root level
- Contains one JSON file per unit category
- Files are embedded/copied to Infrastructure project output
- Loaded at application startup by Infrastructure layer

#### Key Architectural Benefits

- **Testability**: Each layer can be tested independently
- **Maintainability**: Clear separation of concerns
- **Flexibility**: Easy to swap implementations (e.g., JSON storage to database)
- **Scalability**: Layers can be scaled independently if needed
- **SOLID Compliance**: Architecture enforces SOLID principles

### 4.3 Deployment

- **Backend**: Azure App Service
- **Frontend**: Azure Static Web Apps
- **CI/CD**: GitHub Actions

---

## 5. Unit Categories to Support

### Initial MVP Categories (Phase 1)

#### Length / Distance
- **Default (SI Base Unit)**: meter (m) ⭐
- **SI Units**: meter (m), kilometer (km), centimeter (cm), millimeter (mm)
- **Non-SI Units**: foot (ft), inch (in), mile (mi), yard (yd), nautical mile (nmi)

#### Mass / Weight
- **Default (SI Base Unit)**: kilogram (kg) ⭐
- **SI Units**: kilogram (kg), gram (g), metric ton (t)
- **Non-SI Units**: pound (lb), ounce (oz), stone (st), ton (US/UK)

#### Volume
- **Default (SI Derived Unit)**: cubic meter (m³) ⭐
- **SI Units**: cubic meter (m³), liter (L), milliliter (mL)
- **Non-SI Units**: gallon (US/UK), quart (qt), pint (pt), cup, fluid ounce (fl oz), cubic foot (ft³), cubic inch (in³)

#### Area
- **Default (SI Derived Unit)**: square meter (m²) ⭐
- **SI Units**: square meter (m²), hectare (ha), square kilometer (km²)
- **Non-SI Units**: square foot (ft²), square inch (in²), acre, square mile (mi²)

#### Temperature
- **Default (SI Base Unit)**: kelvin (K) ⭐
- **SI Units**: kelvin (K), Celsius (°C) - SI derived unit
- **Non-SI Units**: Fahrenheit (°F), Rankine (°R)

#### Time
- **Default (SI Base Unit)**: second (s) ⭐
- **SI Units**: second (s), millisecond (ms), microsecond (µs)
- **Non-SI Units**: minute (min), hour (h), day (d), week, month, year

#### Speed / Velocity
- **Default (SI Derived Unit)**: meter per second (m/s) ⭐
- **SI Units**: meter per second (m/s), kilometer per hour (km/h)
- **Non-SI Units**: miles per hour (mph), feet per second (ft/s), knot (kn)

### Phase 2 Categories

- Pressure
- Energy / Work
- Power
- Force
- Density
- Angle
- Digital Storage (Bytes, MB, GB, etc.)
- Frequency

### Phase 3 Categories (Advanced / Engineering)

- Flow Rate
- Viscosity (kinematic / dynamic)
- Electromagnetism units
- Radiation units
- Thermal (heat flux, conductivity)

---

## 6. API Contract Proposal

### Request Example

**Endpoint**: `POST /convert`

**Request Body**:
```json
{
  "category": "length",
  "fromUnit": "m",
  "toUnit": "ft",
  "value": 10,
  "locale": "en-US"
}
```

### Response Example

**Success Response**:
```json
{
  "result": 32.8084,
  "formattedResult": "32.8084 ft",
  "precision": 4,
  "formula": "x * 3.28084",
  "fromUnit": {
    "symbol": "m",
    "name": "meter",
    "isBaseUnit": true,
    "isSIUnit": true,
    "unitSystem": "SI"
  },
  "toUnit": {
    "symbol": "ft",
    "name": "foot",
    "isBaseUnit": false,
    "isSIUnit": false,
    "unitSystem": "Imperial"
  }
}
```

### Unit Metadata Endpoint

**Endpoint**: `GET /categories/{name}/units`

**Example**: `GET /categories/length/units`

**Response**:
```json
{
  "category": "length",
  "baseUnit": {
    "symbol": "m",
    "name": "meter",
    "isBaseUnit": true,
    "isSIUnit": true,
    "unitSystem": "SI"
  },
  "units": [
    {
      "symbol": "m",
      "name": "meter",
      "displayName": "Meter",
      "isBaseUnit": true,
      "isSIUnit": true,
      "unitSystem": "SI",
      "conversionFactor": 1.0
    },
    {
      "symbol": "km",
      "name": "kilometer",
      "displayName": "Kilometer",
      "isBaseUnit": false,
      "isSIUnit": true,
      "unitSystem": "SI",
      "conversionFactor": 1000.0
    },
    {
      "symbol": "ft",
      "name": "foot",
      "displayName": "Foot",
      "isBaseUnit": false,
      "isSIUnit": false,
      "unitSystem": "Imperial",
      "conversionFactor": 0.3048
    }
  ]
}
```

**Error Response** (Example):
```json
{
  "error": "Invalid unit combination",
  "message": "Cannot convert between different categories",
  "code": "INVALID_CONVERSION"
}
```

---

## 7. Roadmap

### Phase 1 – MVP (Core)

- ✅ API with base categories
- ✅ React UI + localization (EN + CN)
- ✅ Azure deployment
- ✅ Open-source repo with documentation
- ✅ **Swagger/OpenAPI documentation** with interactive API testing
- ✅ **Over 95% unit test coverage** for all layers
- ✅ **Over 95% integration test coverage** for all API endpoints
- ✅ CI/CD pipeline with test execution and coverage validation

### Phase 2 – Extended Features

- Additional unit categories
- Favorites + history features
- More languages support
- API rate limiting
- Enhanced public API documentation (public Swagger UI access, API documentation portal)

### Phase 3 – Advanced / Community Driven

- Support custom user-defined units
- Graphs for conversion ranges
- Developer plugin ecosystem

---

## 8. Cost Considerations (Azure)

### Recommended Cost-Effective Setup

- **Backend**: 
  - Azure Functions (Consumption Plan) for lowest cost (pay per execution), or
  - Small Azure App Service B1 for always-on backend
- **Frontend**: Azure Static Web Apps Free Tier
- **Storage**: Azure Table Storage or Blob Storage (extremely low-cost)
- **CI/CD**: GitHub Actions (free tier)

### Expected Monthly Cost

You should be able to run the entire app for **a few dollars per month** or even **near zero during development**.

---

## Appendix

### Key Design Principles

1. **Simplicity**: Easy to use and understand
2. **Performance**: Fast response times
3. **Standards-Based**: Uses SI (International System of Units) as the foundation
4. **Extensibility**: Easy to add new units and categories
5. **Cost-Effective**: Minimal infrastructure costs
6. **Open Source**: Community-driven development
7. **Clean Code**: Follows SOLID principles and Clean Architecture

### Architectural Principles

#### SOLID Principles

The backend implementation strictly adheres to SOLID principles:

- **Single Responsibility**: Each class has one reason to change
  - Example: `UnitConverter` handles conversion logic only, `UnitRepository` handles data access only

- **Open/Closed**: Open for extension, closed for modification
  - Example: New unit types can be added by extending `Unit` base class without modifying existing code

- **Liskov Substitution**: Derived classes must be substitutable for base classes
  - Example: Any implementation of `IUnitRepository` can replace another without breaking functionality

- **Interface Segregation**: Clients should not depend on interfaces they don't use
  - Example: Separate interfaces for `IUnitReader` and `IUnitWriter` instead of one large `IUnitRepository`

- **Dependency Inversion**: Depend on abstractions, not concretions
  - Example: Application layer depends on `IUnitRepository` interface (Domain), not `JsonUnitRepository` implementation (Infrastructure)

#### Clean Architecture Benefits

- **Testability**: Each layer can be unit tested independently with mocks
- **Maintainability**: Clear separation makes code easier to understand and modify
- **Flexibility**: Easy to swap implementations (e.g., JSON storage → Database → Cloud storage)
- **Independence**: Business logic (Domain) is independent of frameworks, UI, and databases
- **Scalability**: Layers can evolve independently as requirements change

### SI Units Standard

The application follows the **International System of Units (SI)** standard, which is:
- The most widely used system of measurement worldwide
- Recognized by international standards organizations (BIPM, ISO)
- Used in science, engineering, and international trade
- Provides a consistent base for accurate conversions

All conversions internally use SI base units, ensuring accuracy and consistency across all unit systems (SI, Imperial, US Customary, etc.).
