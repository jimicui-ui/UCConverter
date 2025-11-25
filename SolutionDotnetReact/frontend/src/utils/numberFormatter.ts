/**
 * Format a number with appropriate formatting based on its size
 * Uses thousand separators for normal numbers
 * Uses scientific notation for very large or very small numbers
 * 
 * @param value - The number to format
 * @param locale - Locale string (e.g., 'en-US', 'zh-CN')
 * @param maxFractionDigits - Maximum fraction digits (default: 10)
 * @returns Formatted number string
 */
export function formatNumber(
  value: number,
  locale: string = 'en-US',
  maxFractionDigits: number = 10
): string {
  // Handle special cases
  if (value === 0) return '0';
  if (!isFinite(value)) {
    if (isNaN(value)) return 'NaN';
    return value > 0 ? '∞' : '-∞';
  }

  const absValue = Math.abs(value);
  
  // Use scientific notation for very large or very small numbers
  // Threshold: numbers >= 1e15 or <= 1e-6 (excluding 0)
  if (absValue >= 1e15 || (absValue < 1e-6 && absValue > 0)) {
    return value.toExponential(6);
  }

  // For normal numbers, use locale-aware formatting with thousand separators
  return value.toLocaleString(locale, {
    maximumFractionDigits: maxFractionDigits,
    minimumFractionDigits: 0,
    useGrouping: true, // Enable thousand separators
  });
}

/**
 * Format a number for display in result, with smart formatting
 * 
 * @param value - The number to format
 * @param locale - Locale string
 * @returns Formatted number string
 */
export function formatResultNumber(value: number, locale: string = 'en-US'): string {
  return formatNumber(value, locale, 10);
}

/**
 * Validate if a string is a valid number
 * 
 * @param value - String to validate
 * @returns Object with isValid flag and parsed number (if valid)
 */
export function validateNumber(value: string): { isValid: boolean; number?: number; error?: string } {
  if (!value || value.trim() === '') {
    return { isValid: false, error: 'empty' };
  }

  const trimmed = value.trim();
  const numValue = parseFloat(trimmed);

  if (isNaN(numValue)) {
    return { isValid: false, error: 'invalid' };
  }

  if (!isFinite(numValue)) {
    return { isValid: false, error: 'infinite' };
  }

  return { isValid: true, number: numValue };
}

