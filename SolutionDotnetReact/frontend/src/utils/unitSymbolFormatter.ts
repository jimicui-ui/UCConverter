/**
 * Format unit symbols for display, handling special characters and superscripts
 * 
 * @param symbol - The unit symbol to format
 * @returns Formatted symbol string (may contain HTML entities)
 */
export function formatUnitSymbol(symbol: string): string {
  if (!symbol) return '';
  
  // Replace common patterns with proper formatting
  return symbol
    // Handle superscript numbers (m², m³, etc.)
    .replace(/([a-zA-Z])(\d+)/g, (_match, letter, num) => {
      // Convert numbers to superscript
      const superscriptMap: { [key: string]: string } = {
        '0': '⁰', '1': '¹', '2': '²', '3': '³', '4': '⁴',
        '5': '⁵', '6': '⁶', '7': '⁷', '8': '⁸', '9': '⁹'
      };
      const superscript = num.split('').map((d: string) => superscriptMap[d] || d).join('');
      return letter + superscript;
    })
    // Ensure special characters are preserved
    .replace(/·/g, '·') // Middle dot
    .replace(/Ω/g, 'Ω') // Omega
    .replace(/µ/g, 'µ') // Micro
    .replace(/°/g, '°') // Degree
    .replace(/×/g, '×'); // Multiplication sign
}

/**
 * Format unit symbol for HTML display (with proper escaping)
 * 
 * @param symbol - The unit symbol to format
 * @returns Formatted symbol safe for HTML
 */
export function formatUnitSymbolForHTML(symbol: string): string {
  if (!symbol) return '';
  
  // First apply basic formatting
  let formatted = formatUnitSymbol(symbol);
  
  // Escape HTML entities
  formatted = formatted
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;')
    .replace(/'/g, '&#39;');
  
  return formatted;
}

