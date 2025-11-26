import { useState, useEffect, useMemo } from 'react';
import { useTranslation } from 'react-i18next';
import { apiService } from '../services/api';
import type { CategoryDto, UnitDto, ConvertRequestDto } from '../types/api';
import { ThemeToggle } from './ThemeToggle';
import { useDebounce } from '../hooks/useDebounce';
import { formatResultNumber, validateNumber } from '../utils/numberFormatter';
import { formatUnitSymbol } from '../utils/unitSymbolFormatter';
import './UnitConverter.css';

const STORAGE_KEY_PREFIX = 'uc_lastUnits_';
const DEBOUNCE_DELAY = 500; // milliseconds

export function UnitConverter() {
  const { t, i18n, ready } = useTranslation();
  const [categories, setCategories] = useState<CategoryDto[]>([]);
  const [selectedCategory, setSelectedCategory] = useState<string>('');
  const [units, setUnits] = useState<UnitDto[]>([]);
  const [fromUnit, setFromUnit] = useState<string>('');
  const [toUnit, setToUnit] = useState<string>('');
  const [value, setValue] = useState<string>('');
  const [result, setResult] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [valueError, setValueError] = useState<string | null>(null);
  const [languageChanging, setLanguageChanging] = useState(false);
  const [copied, setCopied] = useState(false);
  const [unitSearchFrom, setUnitSearchFrom] = useState<string>('');
  const [unitSearchTo, setUnitSearchTo] = useState<string>('');
  const [categorySearch, setCategorySearch] = useState<string>('');
  const [selectedGroup, setSelectedGroup] = useState<string>('All');

  // Debounced value for real-time conversion
  const debouncedValue = useDebounce(value, DEBOUNCE_DELAY);

  // Helper to safely get translation
  const safeT = (key: string, fallback?: string) => {
    try {
      return ready ? t(key) : (fallback || key);
    } catch {
      return fallback || key;
    }
  };

  // Get locale string for number formatting
  const locale = i18n.language === 'zh' ? 'zh-CN' : i18n.language === 'fr' ? 'fr-FR' : 'en-US';

  // Enhanced language switching with smooth transitions
  const changeLanguage = (lng: string) => {
    try {
      setLanguageChanging(true);
      i18n.changeLanguage(lng);
      if (typeof window !== 'undefined' && window.localStorage) {
        localStorage.setItem('language', lng);
      }
      // Reset language changing state after a short delay
      // Categories and units will be reloaded automatically by useEffect hooks
      setTimeout(() => setLanguageChanging(false), 300);
    } catch (err) {
      console.error('Error changing language:', err);
      setLanguageChanging(false);
    }
  };

  // Load categories on mount and when language changes
  useEffect(() => {
    const timer = setTimeout(() => {
      loadCategories().catch(err => {
        console.error('Error loading categories:', err);
        setError('Failed to load categories. Please check if the backend is running.');
      });
    }, 100);
    return () => clearTimeout(timer);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [i18n.language]);

  // Load units when category changes or language changes
  useEffect(() => {
    if (selectedCategory) {
      loadUnits(selectedCategory).catch(err => {
        console.error('Error loading units:', err);
        setError('Failed to load units. Please check if the backend is running.');
      });
    } else {
      setUnits([]);
      setFromUnit('');
      setToUnit('');
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedCategory, i18n.language]);

  // Load last used units from localStorage when category changes
  useEffect(() => {
    if (selectedCategory && units.length > 0) {
      const storageKey = `${STORAGE_KEY_PREFIX}${selectedCategory}`;
      const saved = localStorage.getItem(storageKey);
      if (saved) {
        try {
          const { from, to } = JSON.parse(saved);
          // Verify units still exist
          if (units.some(u => u.symbol === from) && units.some(u => u.symbol === to)) {
            setFromUnit(from);
            setToUnit(to);
            return;
          }
        } catch (e) {
          console.error('Error loading saved units:', e);
        }
      }
      // Default: first and second unit
      if (units.length > 0) {
        setFromUnit(units[0].symbol);
        if (units.length > 1) {
          setToUnit(units[1].symbol);
        } else {
          setToUnit(units[0].symbol);
        }
      }
    }
  }, [selectedCategory, units]);

  // Save last used units to localStorage
  const saveLastUsedUnits = (category: string, from: string, to: string) => {
    try {
      const storageKey = `${STORAGE_KEY_PREFIX}${category}`;
      localStorage.setItem(storageKey, JSON.stringify({ from, to }));
    } catch (e) {
      console.error('Error saving units:', e);
    }
  };

  const loadCategories = async () => {
    try {
      setError(null);
      const data = await apiService.getCategories(i18n.language);
      if (data && Array.isArray(data) && data.length > 0) {
        setCategories(data);
        if (data[0] && data[0].name) {
          setSelectedCategory(data[0].name);
        }
      } else {
        setCategories([]);
        setError('No categories available');
      }
    } catch (err: unknown) {
      const errorMsg = safeT('errors.failedToLoadCategories', 'Failed to load categories');
      setError(errorMsg);
      console.error('Failed to load categories:', err);
    }
  };

  const loadUnits = async (categoryName: string) => {
    try {
      setError(null);
      const data = await apiService.getUnitsByCategory(categoryName, i18n.language);
      setUnits(data);
    } catch (err: unknown) {
      const errorMsg = ready ? t('errors.failedToLoadUnits') : 'Failed to load units';
      setError(errorMsg);
      console.error('Failed to load units:', err);
    }
  };

  // Real-time conversion function
  const performConversion = async (val: string, from: string, to: string, category: string) => {
    // Validate input
    const validation = validateNumber(val);
    if (!validation.isValid) {
      if (validation.error === 'empty') {
        setValueError(null);
        setResult(null);
        return;
      }
      setValueError(safeT('errors.invalidNumber', 'Invalid number'));
      setResult(null);
      return;
    }

    if (!category || !from || !to || validation.number === undefined) {
      setValueError(null);
      setResult(null);
      return;
    }

    setValueError(null);
    setLoading(true);
    setError(null);

    try {
      const request: ConvertRequestDto = {
        value: validation.number,
        fromUnit: from,
        toUnit: to,
        category: category,
        locale: i18n.language,
      };

      const response = await apiService.convert(request);
      setResult(response.result);
      // Save last used units
      saveLastUsedUnits(category, from, to);
    } catch (err: unknown) {
      const errorMsg = ready ? t('errors.conversionFailed') : 'Conversion failed';
      setError(errorMsg);
      setResult(null);
      console.error('Conversion failed:', err);
    } finally {
      setLoading(false);
    }
  };

  // Real-time conversion effect - triggers on debounced value or unit changes
  useEffect(() => {
    if (selectedCategory && fromUnit && toUnit && debouncedValue) {
      performConversion(debouncedValue, fromUnit, toUnit, selectedCategory);
    } else {
      setResult(null);
      setLoading(false);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [debouncedValue, fromUnit, toUnit, selectedCategory, i18n.language]);

  // Handle immediate conversion when units change (not debounced)
  useEffect(() => {
    if (selectedCategory && fromUnit && toUnit && value) {
      const validation = validateNumber(value);
      if (validation.isValid && validation.number !== undefined) {
        performConversion(value, fromUnit, toUnit, selectedCategory);
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [fromUnit, toUnit, selectedCategory]);

  // Validate value on change
  const handleValueChange = (newValue: string) => {
    setValue(newValue);
    const validation = validateNumber(newValue);
    if (newValue && !validation.isValid) {
      setValueError(safeT('errors.invalidNumber', 'Invalid number'));
    } else {
      setValueError(null);
    }
  };

  // Manual convert button (fallback)
  const handleConvert = async () => {
    if (!selectedCategory || !fromUnit || !toUnit || !value) {
      setError(safeT('errors.fillAllFields', 'Please fill in all fields'));
      return;
    }

    const validation = validateNumber(value);
    if (!validation.isValid || validation.number === undefined) {
      setValueError(safeT('errors.invalidNumber', 'Invalid number'));
      return;
    }

    await performConversion(value, fromUnit, toUnit, selectedCategory);
  };

  const handleSwap = () => {
    const temp = fromUnit;
    setFromUnit(toUnit);
    setToUnit(temp);
    // Save swapped units
    if (selectedCategory) {
      saveLastUsedUnits(selectedCategory, toUnit, fromUnit);
    }
  };

  // Copy to clipboard
  const handleCopyResult = async () => {
    if (result === null || !value) return;

    const resultText = `${formatResultNumber(parseFloat(value), locale)} ${formatUnitSymbol(fromUnit)} = ${formatResultNumber(result, locale)} ${formatUnitSymbol(toUnit)}`;
    
    try {
      await navigator.clipboard.writeText(resultText);
      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    } catch (err) {
      console.error('Failed to copy:', err);
      // Fallback for older browsers
      const textArea = document.createElement('textarea');
      textArea.value = resultText;
      textArea.style.position = 'fixed';
      textArea.style.opacity = '0';
      document.body.appendChild(textArea);
      textArea.select();
      try {
        document.execCommand('copy');
        setCopied(true);
        setTimeout(() => setCopied(false), 2000);
      } catch (e) {
        console.error('Fallback copy failed:', e);
      }
      document.body.removeChild(textArea);
    }
  };

  // Filter units based on search
  const filteredUnitsFrom = useMemo(() => {
    if (!unitSearchFrom.trim()) return units;
    const searchLower = unitSearchFrom.toLowerCase();
    return units.filter(unit => 
      unit.displayName.toLowerCase().includes(searchLower) ||
      unit.symbol.toLowerCase().includes(searchLower) ||
      (unit.name && unit.name.toLowerCase().includes(searchLower))
    );
  }, [units, unitSearchFrom]);

  const filteredUnitsTo = useMemo(() => {
    if (!unitSearchTo.trim()) return units;
    const searchLower = unitSearchTo.toLowerCase();
    return units.filter(unit => 
      unit.displayName.toLowerCase().includes(searchLower) ||
      unit.symbol.toLowerCase().includes(searchLower) ||
      (unit.name && unit.name.toLowerCase().includes(searchLower))
    );
  }, [units, unitSearchTo]);

  // Get unique groups from categories
  const availableGroups = useMemo(() => {
    const groups = new Set<string>();
    categories.forEach(cat => {
      if (cat?.group) {
        groups.add(cat.group);
      }
    });
    return Array.from(groups).sort();
  }, [categories]);

  // Sort categories alphabetically by displayName
  const sortedCategories = useMemo(() => {
    return [...categories].sort((a, b) => {
      const aName = a?.displayName || a?.name || '';
      const bName = b?.displayName || b?.name || '';
      return aName.localeCompare(bName, i18n.language, { sensitivity: 'base' });
    });
  }, [categories, i18n.language]);

  // Filter categories based on group and search
  const filteredCategories = useMemo(() => {
    let filtered = sortedCategories;
    
    // Filter by group
    if (selectedGroup !== 'All') {
      filtered = filtered.filter(cat => cat?.group === selectedGroup);
    }
    
    // Filter by search
    if (categorySearch.trim()) {
      const searchLower = categorySearch.toLowerCase();
      filtered = filtered.filter(cat => 
        (cat?.displayName || '').toLowerCase().includes(searchLower) ||
        (cat?.name || '').toLowerCase().includes(searchLower)
      );
    }
    
    return filtered;
  }, [sortedCategories, selectedGroup, categorySearch]);

  const currentCategory = categories.find((c) => c && c.name === selectedCategory);
  const baseUnit = units.find(u => u && u.isBaseUnit);

  return (
    <div className={`unit-converter ${languageChanging ? 'language-changing' : ''}`}>
      <a href="#main-content" className="skip-link">{t('accessibility.skipToMainContent')}</a>
      <header className="converter-header" role="banner">
        <div className="header-top">
          <h1>{t('unitConverter.title')}</h1>
          <div className="header-controls" role="toolbar" aria-label={t('common.headerControls') || 'Header controls'}>
            <ThemeToggle />
            <div className="language-selector">
              <label htmlFor="language">{t('common.language')}:</label>
              <select
                id="language"
                value={i18n.language}
                onChange={(e) => changeLanguage(e.target.value)}
                className="language-select"
                disabled={languageChanging}
                aria-label={t('common.selectLanguage') || 'Select language'}
                aria-busy={languageChanging}
              >
                <option value="en">English</option>
                <option value="zh">中文</option>
                <option value="fr">Français</option>
              </select>
              {languageChanging && (
                <span className="language-loading" aria-live="polite" aria-atomic="true">
                  {t('common.loading')}
                </span>
              )}
            </div>
          </div>
        </div>
        <p className="subtitle">{t('unitConverter.subtitle')}</p>
      </header>

      {error && (
        <div className="error-message" role="alert" aria-live="assertive" aria-atomic="true">
          {error}
        </div>
      )}

      <main id="main-content" className="converter-form" role="main" aria-label={t('unitConverter.title') || 'Unit converter'}>
        <div className="form-group">
          <label htmlFor="category">{t('common.category')}</label>
          
          {/* Group Selection Radio Buttons */}
          {availableGroups.length > 0 && (
            <div className="group-selection" role="radiogroup" aria-label="Select converter group">
              <label className="radio-option">
                <input
                  type="radio"
                  name="group"
                  value="All"
                  checked={selectedGroup === 'All'}
                  onChange={(e) => {
                    setSelectedGroup(e.target.value);
                    setCategorySearch('');
                  }}
                  aria-label="All groups"
                />
                <span>{t('groups.all', 'All')}</span>
              </label>
              {availableGroups.map(group => (
                <label key={group} className="radio-option">
                  <input
                    type="radio"
                    name="group"
                    value={group}
                    checked={selectedGroup === group}
                    onChange={(e) => {
                      setSelectedGroup(e.target.value);
                      setCategorySearch('');
                    }}
                    aria-label={`${group} group`}
                  />
                  <span>{t(`groups.${group.toLowerCase()}`, group)}</span>
                </label>
              ))}
            </div>
          )}
          
          {filteredCategories.length > 10 && (
            <input
              type="text"
              className="category-search"
              placeholder="Search categories..."
              value={categorySearch}
              onChange={(e) => setCategorySearch(e.target.value)}
              aria-label="Search categories"
              aria-controls="category"
            />
          )}
          <select
            id="category"
            value={selectedCategory}
            onChange={(e) => {
              setSelectedCategory(e.target.value);
              setResult(null);
              setValue('');
              setValueError(null);
              setUnitSearchFrom('');
              setUnitSearchTo('');
              setCategorySearch('');
            }}
            aria-label={t('common.selectCategory') || 'Select category'}
            aria-required="true"
          >
            {filteredCategories.map((category) => (
              category && (
                <option key={category.name} value={category.name}>
                  {category.displayName || category.name}
                </option>
              )
            ))}
          </select>
        </div>

        <div className="conversion-row">
          <div className="form-group">
            <label htmlFor="fromUnit">{t('common.from')}</label>
            {units.length > 5 && (
              <input
                type="text"
                className="unit-search"
                placeholder={t('common.searchUnits')}
                value={unitSearchFrom}
                onChange={(e) => setUnitSearchFrom(e.target.value)}
                aria-label={t('common.searchFromUnits') || 'Search from units'}
                aria-controls="fromUnit"
              />
            )}
            <select
              id="fromUnit"
              value={fromUnit}
              onChange={(e) => {
                setFromUnit(e.target.value);
                if (selectedCategory) {
                  saveLastUsedUnits(selectedCategory, e.target.value, toUnit);
                }
              }}
              disabled={!selectedCategory || units.length === 0}
              aria-label={t('common.selectFromUnit') || 'Select from unit'}
              aria-required="true"
              aria-describedby={units.length > 5 ? 'fromUnit-search-desc' : undefined}
            >
              {filteredUnitsFrom.length > 0 ? (
                filteredUnitsFrom.map((unit) => (
                  <option key={unit.symbol} value={unit.symbol}>
                    {unit.displayName} ({formatUnitSymbol(unit.symbol)})
                    {unit.isSIUnit && ` [${t('units.si')}]`}
                    {unit.isBaseUnit && ` [${t('units.base')}]`}
                  </option>
                ))
              ) : (
                <option value="">{t('common.noUnitsFound')}</option>
              )}
            </select>
          </div>

          <button
            className="swap-button"
            onClick={handleSwap}
            disabled={!fromUnit || !toUnit}
            aria-label={t('common.swapUnits') || t('common.swap')}
            title={t('common.swapUnits') || t('common.swap')}
            aria-keyshortcuts="s"
            type="button"
          >
            <span aria-hidden="true">⇄</span>
            <span className="sr-only">{t('common.swapUnits') || t('common.swap')}</span>
          </button>

          <div className="form-group">
            <label htmlFor="toUnit">{t('common.to')}</label>
            {units.length > 5 && (
              <input
                type="text"
                className="unit-search"
                placeholder={t('common.searchUnits')}
                value={unitSearchTo}
                onChange={(e) => setUnitSearchTo(e.target.value)}
                aria-label={t('common.searchToUnits') || 'Search to units'}
                aria-controls="toUnit"
              />
            )}
            <select
              id="toUnit"
              value={toUnit}
              onChange={(e) => {
                setToUnit(e.target.value);
                if (selectedCategory) {
                  saveLastUsedUnits(selectedCategory, fromUnit, e.target.value);
                }
              }}
              disabled={!selectedCategory || units.length === 0}
              aria-label={t('common.selectToUnit') || 'Select to unit'}
              aria-required="true"
              aria-describedby={units.length > 5 ? 'toUnit-search-desc' : undefined}
            >
              {filteredUnitsTo.length > 0 ? (
                filteredUnitsTo.map((unit) => (
                  <option key={unit.symbol} value={unit.symbol}>
                    {unit.displayName} ({formatUnitSymbol(unit.symbol)})
                    {unit.isSIUnit && ` [${t('units.si')}]`}
                    {unit.isBaseUnit && ` [${t('units.base')}]`}
                  </option>
                ))
              ) : (
                <option value="">{t('common.noUnitsFound')}</option>
              )}
            </select>
          </div>
        </div>

        <div className="value-row">
          <div className="form-group">
            <label htmlFor="value">
              {t('common.value')}
              {valueError && <span className="error-inline"> *</span>}
            </label>
            <input
              id="value"
              type="number"
              value={value}
              onChange={(e) => handleValueChange(e.target.value)}
              placeholder={t('common.enterValue')}
              step="any"
              className={valueError ? 'error' : ''}
              aria-invalid={!!valueError}
              aria-describedby={valueError ? 'value-error' : undefined}
            />
            {valueError && (
              <span id="value-error" className="error-inline-message" role="alert">
                {valueError}
              </span>
            )}
          </div>

          <button
            className={`convert-button ${loading ? 'loading' : ''}`}
            onClick={handleConvert}
            disabled={loading || !value || !fromUnit || !toUnit}
            title={t('common.convert')}
            aria-label={loading ? t('common.converting') : t('common.convert')}
            aria-busy={loading}
            type="button"
          >
            {loading ? t('common.converting') : t('common.convert')}
          </button>
        </div>

        {loading && (
          <div className="conversion-loading" role="status" aria-live="polite" aria-atomic="true">
            <span>{t('common.converting')}</span>
          </div>
        )}

        {result !== null && !loading && (
          <section className="result" role="region" aria-label={t('common.result') || 'Conversion result'}>
            <div className="result-header">
              <div className="result-label">{t('common.result')}</div>
              <button
                className="copy-button"
                onClick={handleCopyResult}
                title={copied ? t('common.copied') : t('common.copy')}
                aria-label={copied ? t('common.copied') : t('common.copyResult') || t('common.copy')}
                aria-pressed={copied}
                type="button"
              >
                <span aria-hidden="true">{copied ? '✓' : '📋'}</span>
                <span className="sr-only">{copied ? t('common.copied') : t('common.copy')}</span>
              </button>
            </div>
            <div className="result-value" aria-live="polite" aria-atomic="true">
              {formatResultNumber(parseFloat(value), locale)} <span className="unit-symbol">{formatUnitSymbol(fromUnit)}</span> ={' '}
              <strong>{formatResultNumber(result, locale)}</strong> <span className="unit-symbol">{formatUnitSymbol(toUnit)}</span>
            </div>
          </section>
        )}

        {currentCategory && (
          <aside className="category-info" aria-label={t('common.categoryInformation') || 'Category information'}>
            <div className="category-info-item">
              <span className="category-info-label">{t('common.availableUnits')}</span>
              <span className="category-info-value" aria-label={`${units.length} ${t('common.availableUnits')}`}>
                {units.length}
              </span>
            </div>
            {baseUnit && (
              <div className="category-info-item">
                <span className="category-info-label">{t('common.baseUnit')}</span>
                <span className="category-info-value">
                  {baseUnit.displayName || baseUnit.name} (<span className="unit-symbol">{formatUnitSymbol(baseUnit.symbol)}</span>)
                  {baseUnit.isSIUnit && ` [${t('units.si')}]`}
                </span>
              </div>
            )}
          </aside>
        )}
      </main>
    </div>
  );
}
