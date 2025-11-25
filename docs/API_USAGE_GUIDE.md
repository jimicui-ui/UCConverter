# Unit Converter API - Usage Guide

## Table of Contents

1. [Getting Started](#getting-started)
2. [Base URL and Endpoints](#base-url-and-endpoints)
3. [Authentication](#authentication)
4. [Localization](#localization)
5. [Common Use Cases](#common-use-cases)
6. [Integration Examples](#integration-examples)
7. [Error Handling](#error-handling)
8. [OpenAPI Specification](#openapi-specification)

---

## Getting Started

### Base URL

**Development:**
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5185`

**Production:**
- Base URL will be provided when deployed

### Quick Start

1. **Get available categories**: `GET /api/categories`
2. **Get units for a category**: `GET /api/categories/{name}/units`
3. **Perform conversion**: `POST /api/convert`

### Interactive Documentation

Access Swagger UI at `/swagger` when the API is running:
- Development: `https://localhost:5185/swagger`
- Test endpoints directly in the browser
- View request/response schemas
- Try out examples

---

## Base URL and Endpoints

### Endpoints Overview

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/categories` | Get all available unit categories |
| `GET` | `/api/categories/{name}/units` | Get units for a specific category |
| `POST` | `/api/convert` | Convert a value from one unit to another |

### Swagger Documentation

- **Swagger UI**: `/swagger`
- **OpenAPI JSON**: `/swagger/v1/swagger.json`
- **OpenAPI YAML**: Available via Swagger UI export

---

## Authentication

Currently, the API does not require authentication. All endpoints are publicly accessible.

---

## Localization

The API supports localization for error messages and unit display names.

### Supported Locales

- `en` or `en-US` - English (default)
- `zh` or `zh-CN` - Chinese (中文)

### Setting Locale

**Method 1: Query Parameter**
```
GET /api/categories?locale=zh
GET /api/categories/length/units?locale=zh
```

**Method 2: Accept-Language Header**
```
Accept-Language: zh-CN
```

**Method 3: Request Body (for POST /api/convert)**
```json
{
  "category": "length",
  "fromUnit": "m",
  "toUnit": "ft",
  "value": 10.5,
  "locale": "zh"
}
```

---

## Common Use Cases

### Use Case 1: Get All Categories

**Step 1**: Make a GET request to `/api/categories`

**Response:**
```json
[
  {
    "name": "length",
    "displayName": "Length / Distance"
  },
  {
    "name": "weight",
    "displayName": "Weight / Mass"
  },
  {
    "name": "temperature",
    "displayName": "Temperature"
  }
]
```

### Use Case 2: Get Units for a Category

**Step 1**: Choose a category (e.g., "length")

**Step 2**: Make a GET request to `/api/categories/length/units`

**Response:**
```json
[
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
    "symbol": "ft",
    "name": "foot",
    "displayName": "Foot",
    "isBaseUnit": false,
    "isSIUnit": false,
    "unitSystem": "Imperial",
    "conversionFactor": 0.3048
  }
]
```

### Use Case 3: Perform a Conversion

**Step 1**: Get available categories and units (see Use Cases 1 & 2)

**Step 2**: Prepare conversion request:
```json
{
  "category": "length",
  "fromUnit": "m",
  "toUnit": "ft",
  "value": 10.5,
  "locale": "en"
}
```

**Step 3**: Make POST request to `/api/convert`

**Response:**
```json
{
  "result": 34.4488188976378,
  "formattedResult": "34.45",
  "precision": 2,
  "formula": null,
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

### Use Case 4: Temperature Conversion (Formula-Based)

Temperature conversions use formulas instead of simple multiplication factors.

**Request:**
```json
{
  "category": "temperature",
  "fromUnit": "°C",
  "toUnit": "°F",
  "value": 25,
  "locale": "en"
}
```

**Response:**
```json
{
  "result": 77.0,
  "formattedResult": "77.00",
  "precision": 2,
  "formula": "F = C × 9/5 + 32",
  "fromUnit": {
    "symbol": "°C",
    "name": "celsius",
    "isBaseUnit": false,
    "isSIUnit": false,
    "unitSystem": "Metric Non-SI"
  },
  "toUnit": {
    "symbol": "°F",
    "name": "fahrenheit",
    "isBaseUnit": false,
    "isSIUnit": false,
    "unitSystem": "Imperial"
  }
}
```

### Use Case 5: Handle Errors

**Scenario**: Invalid category name

**Request:**
```
GET /api/categories/invalid-category/units
```

**Response (404 Not Found):**
```json
{
  "error": "Category 'invalid-category' not found",
  "category": "invalid-category"
}
```

---

## Integration Examples

### cURL

#### Get Categories
```bash
curl -X GET "https://localhost:5185/api/categories" \
  -H "Accept: application/json"
```

#### Get Units for Category
```bash
curl -X GET "https://localhost:5185/api/categories/length/units?locale=en" \
  -H "Accept: application/json"
```

#### Convert Units
```bash
curl -X POST "https://localhost:5185/api/convert" \
  -H "Content-Type: application/json" \
  -H "Accept: application/json" \
  -d '{
    "category": "length",
    "fromUnit": "m",
    "toUnit": "ft",
    "value": 10.5,
    "locale": "en"
  }'
```

#### Convert with Chinese Locale
```bash
curl -X POST "https://localhost:5185/api/convert" \
  -H "Content-Type: application/json" \
  -H "Accept-Language: zh-CN" \
  -d '{
    "category": "weight",
    "fromUnit": "kg",
    "toUnit": "lb",
    "value": 5,
    "locale": "zh"
  }'
```

### JavaScript/TypeScript (Fetch API)

#### Get Categories
```javascript
async function getCategories(locale = 'en') {
  const response = await fetch(
    `https://localhost:5185/api/categories?locale=${locale}`,
    {
      method: 'GET',
      headers: {
        'Accept': 'application/json'
      }
    }
  );
  
  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }
  
  const categories = await response.json();
  return categories;
}

// Usage
getCategories('en').then(categories => {
  console.log('Categories:', categories);
});
```

#### Get Units for Category
```javascript
async function getUnitsByCategory(categoryName, locale = 'en') {
  const response = await fetch(
    `https://localhost:5185/api/categories/${categoryName}/units?locale=${locale}`,
    {
      method: 'GET',
      headers: {
        'Accept': 'application/json'
      }
    }
  );
  
  if (!response.ok) {
    if (response.status === 404) {
      const error = await response.json();
      throw new Error(error.error || 'Category not found');
    }
    throw new Error(`HTTP error! status: ${response.status}`);
  }
  
  const units = await response.json();
  return units;
}

// Usage
getUnitsByCategory('length', 'en').then(units => {
  console.log('Units:', units);
});
```

#### Convert Units
```javascript
async function convertUnits(request) {
  const response = await fetch('https://localhost:5185/api/convert', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    },
    body: JSON.stringify(request)
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.error || 'Conversion failed');
  }
  
  const result = await response.json();
  return result;
}

// Usage
convertUnits({
  category: 'length',
  fromUnit: 'm',
  toUnit: 'ft',
  value: 10.5,
  locale: 'en'
}).then(result => {
  console.log(`Result: ${result.formattedResult} ${result.toUnit.symbol}`);
});
```

#### Complete Example with Error Handling
```javascript
async function convertWithErrorHandling(category, fromUnit, toUnit, value, locale = 'en') {
  try {
    // Step 1: Get units to validate
    const units = await getUnitsByCategory(category, locale);
    const fromUnitExists = units.some(u => u.symbol === fromUnit);
    const toUnitExists = units.some(u => u.symbol === toUnit);
    
    if (!fromUnitExists || !toUnitExists) {
      throw new Error('Invalid unit symbol');
    }
    
    // Step 2: Perform conversion
    const result = await convertUnits({
      category,
      fromUnit,
      toUnit,
      value,
      locale
    });
    
    return result;
  } catch (error) {
    console.error('Conversion error:', error.message);
    throw error;
  }
}
```

### JavaScript/TypeScript (Axios)

```javascript
import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'https://localhost:5185/api',
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
});

// Get categories
async function getCategories(locale = 'en') {
  const response = await apiClient.get('/categories', {
    params: { locale }
  });
  return response.data;
}

// Get units
async function getUnitsByCategory(categoryName, locale = 'en') {
  try {
    const response = await apiClient.get(`/categories/${categoryName}/units`, {
      params: { locale }
    });
    return response.data;
  } catch (error) {
    if (error.response?.status === 404) {
      throw new Error(error.response.data.error || 'Category not found');
    }
    throw error;
  }
}

// Convert units
async function convertUnits(request) {
  try {
    const response = await apiClient.post('/convert', request);
    return response.data;
  } catch (error) {
    if (error.response?.status === 400 || error.response?.status === 404) {
      throw new Error(error.response.data.error || 'Conversion failed');
    }
    throw error;
  }
}
```

### C# (.NET HttpClient)

```csharp
using System.Net.Http.Json;
using System.Text.Json;

public class UnitConverterApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public UnitConverterApiClient(string baseUrl = "https://localhost:5185")
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
        );
    }

    // Get categories
    public async Task<List<CategoryDto>> GetCategoriesAsync(string locale = "en")
    {
        var response = await _httpClient.GetAsync($"/api/categories?locale={locale}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
    }

    // Get units by category
    public async Task<List<UnitDto>> GetUnitsByCategoryAsync(string categoryName, string locale = "en")
    {
        var response = await _httpClient.GetAsync($"/api/categories/{categoryName}/units?locale={locale}");
        
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            throw new Exception(error?.Error ?? "Category not found");
        }
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<UnitDto>>();
    }

    // Convert units
    public async Task<ConvertResponseDto> ConvertAsync(ConvertRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/convert", request);
        
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest ||
            response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            throw new Exception(error?.Error ?? "Conversion failed");
        }
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ConvertResponseDto>();
    }
}

// DTOs
public class CategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}

public class UnitDto
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsBaseUnit { get; set; }
    public bool IsSIUnit { get; set; }
    public string UnitSystem { get; set; } = string.Empty;
    public double? ConversionFactor { get; set; }
}

public class ConvertRequestDto
{
    public string Category { get; set; } = string.Empty;
    public string FromUnit { get; set; } = string.Empty;
    public string ToUnit { get; set; } = string.Empty;
    public double Value { get; set; }
    public string? Locale { get; set; }
}

public class ConvertResponseDto
{
    public double Result { get; set; }
    public string FormattedResult { get; set; } = string.Empty;
    public int Precision { get; set; }
    public string? Formula { get; set; }
    public UnitInfoDto FromUnit { get; set; } = null!;
    public UnitInfoDto ToUnit { get; set; } = null!;
}

public class UnitInfoDto
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsBaseUnit { get; set; }
    public bool IsSIUnit { get; set; }
    public string UnitSystem { get; set; } = string.Empty;
}

public class ErrorResponse
{
    public string Error { get; set; } = string.Empty;
}

// Usage example
var client = new UnitConverterApiClient("https://localhost:5185");

// Get categories
var categories = await client.GetCategoriesAsync("en");
Console.WriteLine($"Found {categories.Count} categories");

// Get units
var units = await client.GetUnitsByCategoryAsync("length", "en");
Console.WriteLine($"Found {units.Count} units in length category");

// Convert
var result = await client.ConvertAsync(new ConvertRequestDto
{
    Category = "length",
    FromUnit = "m",
    ToUnit = "ft",
    Value = 10.5,
    Locale = "en"
});
Console.WriteLine($"Result: {result.FormattedResult} {result.ToUnit.Symbol}");
```

### Python (requests library)

```python
import requests
from typing import Optional, List, Dict, Any

class UnitConverterApiClient:
    def __init__(self, base_url: str = "https://localhost:5185"):
        self.base_url = base_url
        self.session = requests.Session()
        self.session.headers.update({
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        })
    
    def get_categories(self, locale: str = "en") -> List[Dict[str, Any]]:
        """Get all available unit categories."""
        url = f"{self.base_url}/api/categories"
        params = {"locale": locale}
        response = self.session.get(url, params=params)
        response.raise_for_status()
        return response.json()
    
    def get_units_by_category(self, category_name: str, locale: str = "en") -> List[Dict[str, Any]]:
        """Get units for a specific category."""
        url = f"{self.base_url}/api/categories/{category_name}/units"
        params = {"locale": locale}
        response = self.session.get(url, params=params)
        
        if response.status_code == 404:
            error = response.json()
            raise ValueError(error.get('error', 'Category not found'))
        
        response.raise_for_status()
        return response.json()
    
    def convert(self, category: str, from_unit: str, to_unit: str, 
                value: float, locale: str = "en") -> Dict[str, Any]:
        """Convert a value from one unit to another."""
        url = f"{self.base_url}/api/convert"
        payload = {
            "category": category,
            "fromUnit": from_unit,
            "toUnit": to_unit,
            "value": value,
            "locale": locale
        }
        response = self.session.post(url, json=payload)
        
        if response.status_code in [400, 404]:
            error = response.json()
            raise ValueError(error.get('error', 'Conversion failed'))
        
        response.raise_for_status()
        return response.json()

# Usage example
if __name__ == "__main__":
    client = UnitConverterApiClient("https://localhost:5185")
    
    try:
        # Get categories
        categories = client.get_categories("en")
        print(f"Found {len(categories)} categories")
        for cat in categories:
            print(f"  - {cat['name']}: {cat['displayName']}")
        
        # Get units for length category
        units = client.get_units_by_category("length", "en")
        print(f"\nFound {len(units)} units in length category")
        
        # Convert 10.5 meters to feet
        result = client.convert("length", "m", "ft", 10.5, "en")
        print(f"\nConversion result:")
        print(f"  {result['fromUnit']['symbol']} → {result['toUnit']['symbol']}")
        print(f"  Result: {result['formattedResult']} {result['toUnit']['symbol']}")
        
        # Temperature conversion
        temp_result = client.convert("temperature", "°C", "°F", 25, "en")
        print(f"\nTemperature conversion:")
        print(f"  25°C = {temp_result['formattedResult']}°F")
        
    except ValueError as e:
        print(f"Error: {e}")
    except requests.exceptions.RequestException as e:
        print(f"Request error: {e}")
```

### Python (with error handling and validation)

```python
import requests
from typing import Optional, List, Dict, Any

class UnitConverterApiClient:
    def __init__(self, base_url: str = "https://localhost:5185", 
                 verify_ssl: bool = False):  # Set to True in production
        self.base_url = base_url
        self.session = requests.Session()
        self.session.headers.update({
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        })
        self.session.verify = verify_ssl
    
    def get_categories(self, locale: str = "en") -> List[Dict[str, Any]]:
        """Get all available unit categories."""
        try:
            url = f"{self.base_url}/api/categories"
            params = {"locale": locale}
            response = self.session.get(url, params=params, timeout=10)
            response.raise_for_status()
            return response.json()
        except requests.exceptions.Timeout:
            raise Exception("Request timeout")
        except requests.exceptions.RequestException as e:
            raise Exception(f"Failed to get categories: {e}")
    
    def get_units_by_category(self, category_name: str, locale: str = "en") -> List[Dict[str, Any]]:
        """Get units for a specific category."""
        try:
            url = f"{self.base_url}/api/categories/{category_name}/units"
            params = {"locale": locale}
            response = self.session.get(url, params=params, timeout=10)
            
            if response.status_code == 404:
                error = response.json()
                raise ValueError(f"Category not found: {error.get('error', category_name)}")
            
            response.raise_for_status()
            return response.json()
        except requests.exceptions.Timeout:
            raise Exception("Request timeout")
        except requests.exceptions.RequestException as e:
            raise Exception(f"Failed to get units: {e}")
    
    def convert(self, category: str, from_unit: str, to_unit: str, 
                value: float, locale: str = "en") -> Dict[str, Any]:
        """Convert a value from one unit to another."""
        # Validate input
        if not category or not from_unit or not to_unit:
            raise ValueError("Category, fromUnit, and toUnit are required")
        
        if not isinstance(value, (int, float)):
            raise ValueError("Value must be a number")
        
        try:
            url = f"{self.base_url}/api/convert"
            payload = {
                "category": category,
                "fromUnit": from_unit,
                "toUnit": to_unit,
                "value": value,
                "locale": locale
            }
            response = self.session.post(url, json=payload, timeout=10)
            
            if response.status_code == 400:
                error = response.json()
                raise ValueError(f"Bad request: {error.get('error', 'Invalid conversion')}")
            
            if response.status_code == 404:
                error = response.json()
                raise ValueError(f"Not found: {error.get('error', 'Category or unit not found')}")
            
            response.raise_for_status()
            return response.json()
        except requests.exceptions.Timeout:
            raise Exception("Request timeout")
        except requests.exceptions.RequestException as e:
            raise Exception(f"Conversion failed: {e}")
```

---

## Error Handling

### HTTP Status Codes

| Status Code | Description | When It Occurs |
|-------------|-------------|----------------|
| 200 | OK | Successful request |
| 400 | Bad Request | Missing required fields, invalid conversion |
| 404 | Not Found | Category or unit not found |
| 500 | Internal Server Error | Server-side error |

### Error Response Format

All error responses follow this format:

```json
{
  "error": "Error message description",
  "category": "category-name"  // Optional, for category-related errors
}
```

### Common Error Scenarios

#### 1. Missing Required Field

**Request:**
```json
{
  "category": "length",
  "fromUnit": "m",
  "value": 10.5
  // Missing "toUnit"
}
```

**Response (400 Bad Request):**
```json
{
  "error": "ToUnit is required"
}
```

#### 2. Invalid Category

**Request:**
```
GET /api/categories/invalid-category/units
```

**Response (404 Not Found):**
```json
{
  "error": "Category 'invalid-category' not found",
  "category": "invalid-category"
}
```

#### 3. Invalid Unit Symbol

**Request:**
```json
{
  "category": "length",
  "fromUnit": "invalid-unit",
  "toUnit": "ft",
  "value": 10.5
}
```

**Response (404 Not Found):**
```json
{
  "error": "Unit 'invalid-unit' not found",
  "unit": "invalid-unit"
}
```

#### 4. Units from Different Categories

**Request:**
```json
{
  "category": "length",
  "fromUnit": "m",      // Length unit
  "toUnit": "kg",       // Weight unit - ERROR!
  "value": 10.5
}
```

**Response (400 Bad Request):**
```json
{
  "error": "Invalid conversion: units must be from the same category",
  "fromUnit": "m",
  "toUnit": "kg"
}
```

### Error Handling Best Practices

1. **Always check HTTP status codes** before processing response
2. **Parse error messages** from the response body
3. **Handle timeouts** appropriately
4. **Validate input** before sending requests
5. **Provide user-friendly error messages** in your application

---

## OpenAPI Specification

### Accessing the OpenAPI Specification

The API generates a complete OpenAPI 3.0 specification that can be:

1. **Viewed in Swagger UI**: Navigate to `/swagger` when the API is running
2. **Downloaded as JSON**: Access `/swagger/v1/swagger.json`
3. **Exported from Swagger UI**: Use the "Export" feature in Swagger UI

### Using the OpenAPI Specification

#### Import into API Clients

The OpenAPI specification can be imported into various API clients and code generation tools:

- **Postman**: Import the JSON file directly
- **Insomnia**: Import the JSON file
- **REST Client (VS Code)**: Use the specification to generate requests
- **OpenAPI Generator**: Generate client libraries in multiple languages

#### Code Generation Examples

**Using OpenAPI Generator (Java):**
```bash
openapi-generator generate \
  -i https://localhost:5185/swagger/v1/swagger.json \
  -g java \
  -o ./generated-client
```

**Using OpenAPI Generator (TypeScript):**
```bash
openapi-generator generate \
  -i https://localhost:5185/swagger/v1/swagger.json \
  -g typescript-axios \
  -o ./generated-client
```

**Using OpenAPI Generator (Python):**
```bash
openapi-generator generate \
  -i https://localhost:5185/swagger/v1/swagger.json \
  -g python \
  -o ./generated-client
```

### Specification Features

The OpenAPI specification includes:

- ✅ Complete endpoint definitions
- ✅ Request/response schemas with examples
- ✅ Parameter descriptions and constraints
- ✅ Error response schemas
- ✅ Data type definitions
- ✅ Validation constraints
- ✅ Localization support documentation

---

## Additional Resources

### Supported Categories

- **length** - Length / Distance units
- **weight** - Weight / Mass units
- **temperature** - Temperature units
- **volume** - Volume units
- **area** - Area units
- **time** - Time units
- **speed** - Speed units

### Unit Systems

- **SI** - International System of Units (base units)
- **Imperial** - Imperial units (UK)
- **US Customary** - US Customary units
- **Metric Non-SI** - Metric units not part of SI

### Conversion Types

- **Linear Conversions**: Most units use conversion factors (length, weight, volume, etc.)
- **Formula-Based Conversions**: Temperature conversions use mathematical formulas

---

## Support

For issues, questions, or contributions, please refer to the project repository.

---

**Last Updated**: 2024
**API Version**: v1

