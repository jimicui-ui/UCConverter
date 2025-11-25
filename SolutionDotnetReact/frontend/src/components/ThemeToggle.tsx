import { useTheme } from '../contexts/ThemeContext';
import { useTranslation } from 'react-i18next';
import './ThemeToggle.css';

export function ThemeToggle() {
  const { theme, toggleTheme } = useTheme();
  const { t } = useTranslation();

  const getThemeLabel = () => {
    switch (theme) {
      case 'light':
        return t('common.switchToDark');
      case 'dark':
        return t('common.switchToBlue');
      case 'blue':
        return t('common.switchToLight');
      default:
        return t('common.switchTheme');
    }
  };

  const getThemeIcon = () => {
    switch (theme) {
      case 'light':
        return '🌙';
      case 'dark':
        return '💙';
      case 'blue':
        return '☀️';
      default:
        return '🎨';
    }
  };

  return (
    <button
      className="theme-toggle"
      onClick={toggleTheme}
      aria-label={getThemeLabel()}
      title={getThemeLabel()}
    >
      <span className="theme-icon" aria-hidden="true">{getThemeIcon()}</span>
    </button>
  );
}

