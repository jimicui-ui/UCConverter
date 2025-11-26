# Phase 3 Requirements - French Language Support

## Table of Contents
1. [Overview](#1-overview)
2. [Frontend Localization](#2-frontend-localization)
   - [2.1 Translation Files](#21-translation-files)
   - [2.2 i18next Configuration](#22-i18next-configuration)
   - [2.3 UI Components Updates](#23-ui-components-updates)
3. [Backend Localization](#3-backend-localization)
   - [3.1 Resource Files](#31-resource-files)
   - [3.2 API Configuration](#32-api-configuration)
   - [3.3 Error Messages](#33-error-messages)
4. [Translation Requirements](#4-translation-requirements)
   - [4.1 UI Text Translations](#41-ui-text-translations)
   - [4.2 Unit Names and Categories](#42-unit-names-and-categories)
   - [4.3 Error Messages](#43-error-messages)
5. [Implementation Priorities](#5-implementation-priorities)
6. [Success Criteria](#6-success-criteria)
7. [Testing Requirements](#7-testing-requirements)

---

## 1. Overview

Phase 3 focuses on adding French ("fr") language support to the Unit Converter application. This includes:

- **Frontend Localization**: Adding French translations to the React frontend using i18next
- **Backend Localization**: Adding French resource files for API responses and error messages
- **Complete Translation Coverage**: Ensuring all UI elements, unit names, categories, and error messages are translated
- **Language Selection**: Enabling users to switch to French in the language selector

This phase extends the existing localization infrastructure (English and Chinese) to include French, maintaining consistency with the current implementation approach.

---

## 2. Frontend Localization

### 2.1 Translation Files

#### 2.1.1 Create French Translation File
- **Location**: `frontend/src/i18n/locales/fr.json`
- **Content**: Complete translation of all UI strings from `en.json`
- **Structure**: Maintain the same JSON structure as existing translation files
- **Keys**: All translation keys must match exactly with `en.json` and `zh.json`

#### 2.1.2 Translation Coverage
The French translation file must include:
- **UI Labels**: All buttons, labels, placeholders, and form elements
- **Category Names**: All unit category names (Length, Weight, Temperature, etc.)
- **Unit Names**: All unit display names
- **Messages**: Success messages, error messages, validation messages
- **Navigation**: Menu items, headers, footers
- **Help Text**: Tooltips, descriptions, and help content

### 2.2 i18next Configuration

#### 2.2.1 Update i18n Configuration
- **File**: `frontend/src/i18n/config.ts`
- **Changes Required**:
  - Import French translation file
  - Add French to resources object
  - Ensure French is available in language selector
  - Set appropriate fallback behavior

#### 2.2.2 Language Selector Updates
- **Component**: Language selector component (if exists)
- **Updates**:
  - Add French option to language dropdown/selector
  - Display "Français" or "FR" as the label
  - Ensure proper language code mapping ("fr")

### 2.3 UI Components Updates

#### 2.3.1 Component Verification
- Verify all components use i18next translation keys
- Ensure no hardcoded English text remains
- Check that all user-facing strings are translatable

#### 2.3.2 Language Switching
- Test language switching to French
- Verify translations load correctly
- Ensure UI layout accommodates French text (may be longer than English)
- Check for text overflow or layout issues

---

## 3. Backend Localization

### 3.1 Resource Files

#### 3.1.1 Create French Resource Files
- **Location**: `SolutionDotnetReact/src/UCConverter.Application/Resources/`
- **Files**: 
  - `SharedResources.fr.resx` - Main shared resources
  - Additional resource files as needed for specific modules
- **Structure**: Follow the same structure as existing `SharedResources.en.resx` and `SharedResources.zh.resx`

#### 3.1.2 Translation Coverage
The French resource files must include:
- **Error Messages**: All API error messages
- **Validation Messages**: Input validation error messages
- **Category Names**: Localized category names returned by API
- **Unit Names**: Localized unit display names
- **Success Messages**: Operation success messages (if any)

### 3.2 API Configuration

#### 3.2.1 Update Supported Cultures
- **File**: `SolutionDotnetReact/src/UCConverter.Api/Program.cs`
- **Changes Required**:
  - Add "fr" and "fr-FR" to `supportedCultures` array
  - Ensure French is included in `AddSupportedCultures` and `AddSupportedUICultures`
  - Verify Accept-Language header processing works for French

#### 3.2.2 Locale Parameter Support
- Ensure `?locale=fr` query parameter works for all endpoints
- Verify `Accept-Language: fr` header works correctly
- Test locale fallback behavior (fr → en if translation missing)

### 3.3 Error Messages

#### 3.3.1 Error Message Translation
- All error responses must be translatable to French
- Common error scenarios to translate:
  - Invalid category name
  - Invalid unit symbol
  - Units from different categories
  - Missing required fields
  - Invalid numeric values
  - Conversion errors

#### 3.3.2 Error Response Format
- Maintain consistent error response structure
- Ensure error messages are properly localized
- Test error scenarios with French locale

---

## 4. Translation Requirements

### 4.1 UI Text Translations

#### 4.1.1 Core UI Elements
Translate the following UI elements to French:
- **Page Title**: "Unit Converter" → "Convertisseur d'Unités"
- **Buttons**: "Convert", "Swap", "Clear", etc.
- **Form Labels**: "From", "To", "Value", "Category", etc.
- **Placeholders**: Input field placeholder text
- **Messages**: Loading messages, success messages, etc.

#### 4.1.2 Navigation and Headers
- Application header/title
- Language selector label
- Any navigation menus
- Footer text (if applicable)

### 4.2 Unit Names and Categories

#### 4.2.1 Category Names
Translate all category names:
- **Length** → **Longueur**
- **Weight** → **Masse** (or **Poids**)
- **Temperature** → **Température**
- **Volume** → **Volume**
- **Area** → **Superficie** (or **Aire**)
- **Time** → **Temps**
- **Speed** → **Vitesse**

#### 4.2.2 Unit Names
Translate all unit display names to French:
- **Length Units**: 
  - Meter → Mètre
  - Kilometer → Kilomètre
  - Centimeter → Centimètre
  - Millimeter → Millimètre
  - Inch → Pouce
  - Foot → Pied
  - Yard → Yard
  - Mile → Mille
  - etc.

- **Weight Units**:
  - Kilogram → Kilogramme
  - Gram → Gramme
  - Pound → Livre
  - Ounce → Once
  - etc.

- **Temperature Units**:
  - Celsius → Celsius
  - Fahrenheit → Fahrenheit
  - Kelvin → Kelvin

- **Other Categories**: Translate all units in Volume, Area, Time, and Speed categories

#### 4.2.3 Unit Symbols
- Unit symbols (m, kg, °C, etc.) typically remain unchanged
- Ensure proper display of symbols with French text

### 4.3 Error Messages

#### 4.3.1 Validation Errors
Translate common validation error messages:
- Invalid input format
- Required field missing
- Value out of range
- Invalid unit combination

#### 4.3.2 API Errors
Translate API error messages:
- Category not found
- Unit not found
- Conversion error
- Invalid request format

---

## 5. Implementation Priorities

### Priority 1 (High) - Core Translation Files
1. Create `fr.json` translation file for frontend
2. Create `SharedResources.fr.resx` for backend
3. Update i18next configuration to include French
4. Update API Program.cs to support French locale
5. Translate all core UI elements and category names

### Priority 2 (Medium) - Complete Translation Coverage
1. Translate all unit names
2. Translate all error messages
3. Update language selector to include French
4. Test language switching functionality

### Priority 3 (Low) - Polish & Testing
1. Review translations for accuracy and consistency
2. Test all UI components with French translations
3. Verify API responses in French locale
4. Check for text overflow or layout issues
5. Update documentation to reflect French support

---

## 6. Success Criteria

### 6.1 Frontend Localization
- [ ] French translation file (`fr.json`) created with all required translations
- [ ] i18next configuration updated to include French
- [ ] Language selector includes French option
- [ ] All UI elements display correctly in French
- [ ] Language switching to French works seamlessly
- [ ] No hardcoded English text remains in components
- [ ] UI layout accommodates French text without overflow

### 6.2 Backend Localization
- [ ] French resource files created (`SharedResources.fr.resx`)
- [ ] API supports `fr` and `fr-FR` locales
- [ ] All API endpoints return French translations when `locale=fr` is specified
- [ ] Error messages are properly translated to French
- [ ] Category names are returned in French when requested
- [ ] Unit names are returned in French when requested

### 6.3 Translation Quality
- [ ] All UI text is translated to French
- [ ] All category names are translated accurately
- [ ] All unit names are translated accurately
- [ ] All error messages are translated
- [ ] Translations are grammatically correct and contextually appropriate
- [ ] Technical terms are used consistently

### 6.4 Testing
- [ ] Frontend displays correctly in French
- [ ] Language switching works without page reload issues
- [ ] API returns French content when `locale=fr` is specified
- [ ] API returns French content when `Accept-Language: fr` header is used
- [ ] Error scenarios display French error messages
- [ ] All unit conversions work correctly with French locale
- [ ] No console errors or warnings related to missing translations

### 6.5 Documentation
- [ ] API documentation updated to include French in supported locales
- [ ] README updated to mention French support
- [ ] API usage guide updated with French locale examples

---

## 7. Testing Requirements

### 7.1 Frontend Testing

#### 7.1.1 Language Switching
- Test switching from English to French
- Test switching from Chinese to French
- Test switching from French to other languages
- Verify language preference is persisted (localStorage)
- Test page refresh maintains selected language

#### 7.1.2 UI Display
- Verify all text displays in French
- Check for text overflow or layout issues
- Test on different screen sizes (mobile, tablet, desktop)
- Verify form elements display correctly
- Check dropdown menus and selectors

#### 7.1.3 Functionality
- Test unit conversion with French locale
- Verify category selection works
- Test unit selection dropdowns
- Verify all buttons and interactions work

### 7.2 Backend Testing

#### 7.2.1 API Locale Support
- Test `GET /api/categories?locale=fr` returns French category names
- Test `GET /api/categories/{name}/units?locale=fr` returns French unit names
- Test `POST /api/convert` with French locale in query parameter
- Test `Accept-Language: fr` header works for all endpoints

#### 7.2.2 Error Messages
- Test error scenarios return French error messages
- Verify error message format is consistent
- Test various error conditions (invalid category, invalid unit, etc.)

#### 7.2.3 Integration Testing
- Test end-to-end flow with French locale
- Verify frontend-backend communication with French locale
- Test API responses are properly consumed by frontend

### 7.3 Cross-Browser Testing
- Test French translations in Chrome
- Test French translations in Firefox
- Test French translations in Safari
- Test French translations in Edge

### 7.4 Accessibility Testing
- Verify French translations work with screen readers
- Test keyboard navigation with French UI
- Ensure ARIA labels are properly translated

---

## 8. Technical Considerations

### 8.1 Translation Management
- Consider using a translation management tool for future languages
- Maintain consistency in translation keys across all languages
- Document translation guidelines for future contributors

### 8.2 Text Length Considerations
- French text may be longer than English in some cases
- Ensure UI components accommodate longer text
- Test responsive design with French text

### 8.3 Cultural Considerations
- Use appropriate French terminology for technical terms
- Consider regional variations (France vs. Canada)
- Use standard French (France) as the base, unless specified otherwise

### 8.4 Performance
- Ensure translation files load efficiently
- Verify no performance degradation with additional language
- Test language switching performance

---

## 9. Notes

- All translations should use standard French (France) unless otherwise specified
- Maintain consistency with existing translation patterns (English, Chinese)
- Ensure backward compatibility - existing functionality should not break
- Consider future extensibility for additional languages
- Document any translation decisions or special cases

---

**Document Version**: 1.0  
**Last Updated**: [Current Date]  
**Status**: Draft for Review

