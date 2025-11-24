import { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { apiService } from '../services/api';
import type { CategoryDto, UnitDto, ConvertRequestDto } from '../types/api';
import './UnitConverter.css';

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

  // Helper to safely get translation
  const safeT = (key: string, fallback?: string) => {
    try {
      return ready ? t(key) : (fallback || key);
    } catch {
      return fallback || key;
    }
  };

  const changeLanguage = (lng: string) => {
    try {
      i18n.changeLanguage(lng);
      if (typeof window !== 'undefined' && window.localStorage) {
        localStorage.setItem('language', lng);
      }
      // Reload categories to get localized names
      loadCategories().catch(err => console.error('Error reloading categories:', err));
    } catch (err) {
      console.error('Error changing language:', err);
    }
  };

  // Load categories on mount
  useEffect(() => {
    // Use a small delay to ensure i18n is ready
    const timer = setTimeout(() => {
      loadCategories().catch(err => {
        console.error('Error loading categories:', err);
        setError('Failed to load categories. Please check if the backend is running.');
      });
    }, 100);
    return () => clearTimeout(timer);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  // Load units when category changes
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
      if (data.length > 0) {
        setFromUnit(data[0].symbol);
        if (data.length > 1) {
          setToUnit(data[1].symbol);
        } else {
          setToUnit(data[0].symbol);
        }
      }
    } catch (err: unknown) {
      const errorMsg = ready ? t('errors.failedToLoadUnits') : 'Failed to load units';
      setError(errorMsg);
      console.error('Failed to load units:', err);
    }
  };

  const handleConvert = async () => {
    if (!selectedCategory || !fromUnit || !toUnit || !value) {
      setError(t('errors.fillAllFields'));
      return;
    }

    const numValue = parseFloat(value);
    if (isNaN(numValue)) {
      setError(t('errors.invalidNumber'));
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const request: ConvertRequestDto = {
        value: numValue,
        fromUnit,
        toUnit,
        category: selectedCategory,
        locale: i18n.language,
      };

      const response = await apiService.convert(request);
      setResult(response.result);
    } catch (err: unknown) {
      const errorMsg = ready ? t('errors.conversionFailed') : 'Conversion failed';
      setError(errorMsg);
      setResult(null);
      console.error('Conversion failed:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleSwap = () => {
    const temp = fromUnit;
    setFromUnit(toUnit);
    setToUnit(temp);
    setResult(null);
  };

  const currentCategory = categories.find((c) => c && c.name === selectedCategory);

  return (
    <div className="unit-converter">
      <div className="converter-header">
        <div className="header-top">
          <h1>{t('unitConverter.title')}</h1>
          <div className="language-selector">
            <label htmlFor="language">{t('common.language')}:</label>
            <select
              id="language"
              value={i18n.language}
              onChange={(e) => changeLanguage(e.target.value)}
              className="language-select"
            >
              <option value="en">English</option>
              <option value="zh">中文</option>
            </select>
          </div>
        </div>
        <p className="subtitle">{t('unitConverter.subtitle')}</p>
      </div>

      {error && <div className="error-message">{error}</div>}

      <div className="converter-form">
        <div className="form-group">
          <label htmlFor="category">{t('common.category')}</label>
          <select
            id="category"
            value={selectedCategory}
            onChange={(e) => {
              setSelectedCategory(e.target.value);
              setResult(null);
            }}
          >
            {categories.map((category) => (
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
            <select
              id="fromUnit"
              value={fromUnit}
              onChange={(e) => {
                setFromUnit(e.target.value);
                setResult(null);
              }}
              disabled={!selectedCategory || units.length === 0}
            >
              {units.map((unit) => (
                <option key={unit.symbol} value={unit.symbol}>
                  {unit.displayName} ({unit.symbol})
                  {unit.isSIUnit && ` [${t('units.si')}]`}
                  {unit.isBaseUnit && ` [${t('units.base')}]`}
                </option>
              ))}
            </select>
          </div>

          <button
            className="swap-button"
            onClick={handleSwap}
            disabled={!fromUnit || !toUnit}
            aria-label={t('common.swap')}
            title={t('common.swap')}
          >
            ⇄
          </button>

          <div className="form-group">
            <label htmlFor="toUnit">{t('common.to')}</label>
            <select
              id="toUnit"
              value={toUnit}
              onChange={(e) => {
                setToUnit(e.target.value);
                setResult(null);
              }}
              disabled={!selectedCategory || units.length === 0}
            >
              {units.map((unit) => (
                <option key={unit.symbol} value={unit.symbol}>
                  {unit.displayName} ({unit.symbol})
                  {unit.isSIUnit && ` [${t('units.si')}]`}
                  {unit.isBaseUnit && ` [${t('units.base')}]`}
                </option>
              ))}
            </select>
          </div>
        </div>

        <div className="value-row">
          <div className="form-group">
            <label htmlFor="value">{t('common.value')}</label>
            <input
              id="value"
              type="number"
              value={value}
              onChange={(e) => {
                setValue(e.target.value);
                setResult(null);
              }}
              placeholder={t('common.enterValue')}
              step="any"
            />
          </div>

          <button
            className="convert-button"
            onClick={handleConvert}
            disabled={loading || !value || !fromUnit || !toUnit}
          >
            {loading ? t('common.converting') : t('common.convert')}
          </button>
        </div>

        {result !== null && (
          <div className="result">
            <div className="result-label">{t('common.result')}</div>
            <div className="result-value">
              {parseFloat(value).toLocaleString()} {fromUnit} ={' '}
              <strong>{result.toLocaleString(undefined, { maximumFractionDigits: 10 })}</strong>{' '}
              {toUnit}
            </div>
          </div>
        )}

        {currentCategory && (
          <div className="category-info">
            <p>
              <strong>{t('common.availableUnits')}</strong> {units.length}
            </p>
            {(() => {
              const baseUnit = units.find(u => u && u.isBaseUnit);
              return baseUnit ? (
                <p>
                  <strong>{t('common.baseUnit')}</strong> {baseUnit.displayName || baseUnit.name} (
                  {baseUnit.symbol})
                  {baseUnit.isSIUnit && ` [${t('units.si')}]`}
                </p>
              ) : null;
            })()}
          </div>
        )}
      </div>
    </div>
  );
}

