# US-005: API Documentation - Swagger Examples & Enhancement

## User Story

**As a** developer  
**I want** comprehensive Swagger examples and enhanced Swagger UI features  
**So that** I can easily understand and test the API endpoints

## Description

Add comprehensive Swagger examples for all API endpoints and enhance Swagger UI with pre-filled examples, better response display, and improved error handling. This includes examples for all endpoints, error responses, and Swagger UI enhancements.

## Scope

- Swagger examples for GET /api/categories (with different locales)
- Swagger examples for GET /api/categories/{name}/units (all categories, different locales)
- Comprehensive examples for POST /api/convert (linear conversions, formula-based, all categories, edge cases)
- Error response examples (400, 404, 500)
- Swagger UI enhancements (pre-filled examples, syntax highlighting, copy functionality)
- Enhanced response display with expandable sections

## Acceptance Criteria

- [ ] All endpoints have comprehensive, realistic examples
- [ ] Examples cover different scenarios (success, errors, edge cases)
- [ ] Examples demonstrate localization (English and Chinese)
- [ ] Error responses are documented with examples
- [ ] Swagger UI has pre-filled examples in "Try it out"
- [ ] Response display has syntax highlighting and copy functionality
- [ ] All examples are testable and working
- [ ] Examples are clearly documented

## Priority

**High**

## Dependencies

None - Can be implemented independently (backend work)

## Technical Notes

- Use Swashbuckle attributes to add examples
- Configure Swagger UI for better user experience
- Test all examples in Swagger UI
- Include examples for both English and Chinese locales

## Related Requirements

- Section 3.1: Enhanced Swagger Examples
- Section 3.3.1: Swagger UI Enhancements

