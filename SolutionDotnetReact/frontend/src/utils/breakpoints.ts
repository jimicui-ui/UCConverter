/**
 * Breakpoint constants for responsive design
 * These values match the CSS media query breakpoints
 */
export const BREAKPOINTS = {
  MOBILE_MAX: 767,
  TABLET_MIN: 768,
  TABLET_MAX: 1023,
  DESKTOP_MIN: 1024,
  DESKTOP_MAX: 1919,
  LARGE_DESKTOP_MIN: 1920,
  ULTRA_WIDE_MIN: 2560,
} as const;

/**
 * Check if current viewport is mobile
 */
export function isMobile(): boolean {
  if (typeof window === 'undefined') return false;
  return window.innerWidth <= BREAKPOINTS.MOBILE_MAX;
}

/**
 * Check if current viewport is tablet
 */
export function isTablet(): boolean {
  if (typeof window === 'undefined') return false;
  const width = window.innerWidth;
  return width >= BREAKPOINTS.TABLET_MIN && width <= BREAKPOINTS.TABLET_MAX;
}

/**
 * Check if current viewport is desktop
 */
export function isDesktop(): boolean {
  if (typeof window === 'undefined') return false;
  const width = window.innerWidth;
  return width >= BREAKPOINTS.DESKTOP_MIN && width <= BREAKPOINTS.DESKTOP_MAX;
}

/**
 * Check if current viewport is large desktop
 */
export function isLargeDesktop(): boolean {
  if (typeof window === 'undefined') return false;
  return window.innerWidth >= BREAKPOINTS.LARGE_DESKTOP_MIN;
}

/**
 * Get current device type
 */
export function getDeviceType(): 'mobile' | 'tablet' | 'desktop' | 'large-desktop' {
  if (isMobile()) return 'mobile';
  if (isTablet()) return 'tablet';
  if (isLargeDesktop()) return 'large-desktop';
  return 'desktop';
}

/**
 * Check if device is touch-enabled
 */
export function isTouchDevice(): boolean {
  if (typeof window === 'undefined') return false;
  return 'ontouchstart' in window || navigator.maxTouchPoints > 0;
}

