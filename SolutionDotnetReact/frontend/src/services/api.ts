import type {
  CategoryDto,
  ConvertRequestDto,
  ConvertResponseDto,
  UnitDto,
} from '../types/api';

// Use relative paths in development (goes through Vite proxy)
// Use environment variable in production
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || '';

class ApiService {
  private baseUrl: string;

  constructor(baseUrl: string = API_BASE_URL) {
    // If baseUrl is empty, use relative paths (Vite proxy will handle it)
    this.baseUrl = baseUrl || '';
  }

  /**
   * Get all available unit categories
   */
  async getCategories(locale?: string): Promise<CategoryDto[]> {
    const headers: HeadersInit = {};
    // Map language codes to full locale codes for Accept-Language header and query parameter
    const localeHeader = locale === 'zh' ? 'zh-CN' : locale === 'fr' ? 'fr-FR' : locale;
    if (localeHeader) {
      headers['Accept-Language'] = localeHeader;
    }
    
    const basePath = this.baseUrl || '';
    const url = localeHeader 
      ? `${basePath}/api/categories?locale=${localeHeader}` 
      : `${basePath}/api/categories`;
    const response = await fetch(url, { headers });
    
    if (!response.ok) {
      throw new Error(`Failed to fetch categories: ${response.statusText}`);
    }
    
    return response.json();
  }

  /**
   * Get all units for a specific category
   */
  async getUnitsByCategory(categoryName: string, locale?: string): Promise<UnitDto[]> {
    const headers: HeadersInit = {};
    // Map language codes to full locale codes for Accept-Language header and query parameter
    const localeHeader = locale === 'zh' ? 'zh-CN' : locale === 'fr' ? 'fr-FR' : locale;
    if (localeHeader) {
      headers['Accept-Language'] = localeHeader;
    }
    
    const basePath = this.baseUrl || '';
    const url = localeHeader 
      ? `${basePath}/api/categories/${encodeURIComponent(categoryName)}/units?locale=${localeHeader}`
      : `${basePath}/api/categories/${encodeURIComponent(categoryName)}/units`;
    const response = await fetch(url, { headers });
    
    if (!response.ok) {
      throw new Error(`Failed to fetch units for category ${categoryName}: ${response.statusText}`);
    }
    
    return response.json();
  }

  /**
   * Convert a value from one unit to another
   */
  async convert(request: ConvertRequestDto): Promise<ConvertResponseDto> {
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
    };
    
    // Add locale to headers if provided, mapping language codes to full locale codes
    if (request.locale) {
      const localeHeader = request.locale === 'zh' ? 'zh-CN' : request.locale === 'fr' ? 'fr-FR' : request.locale;
      headers['Accept-Language'] = localeHeader;
    }
    
    const basePath = this.baseUrl || '';
    const response = await fetch(`${basePath}/api/convert`, {
      method: 'POST',
      headers,
      body: JSON.stringify(request),
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`Conversion failed: ${errorText || response.statusText}`);
    }

    return response.json();
  }
}

export const apiService = new ApiService();

