import { UnitConverter } from './components/UnitConverter'
import { ErrorBoundary } from './components/ErrorBoundary'
import { ThemeProvider } from './contexts/ThemeContext'
import './App.css'

function App() {
  return (
    <ErrorBoundary>
      <ThemeProvider>
        <div className="app">
          <UnitConverter />
        </div>
      </ThemeProvider>
    </ErrorBoundary>
  )
}

export default App
