import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './styles/design-tokens.css'
import './index.css'
import './i18n/config'
import App from './App.tsx'

const rootElement = document.getElementById('root')
if (!rootElement) {
  throw new Error('Root element not found. Make sure index.html has a div with id="root"')
}

createRoot(rootElement).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
