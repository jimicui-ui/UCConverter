import { UnitConverter } from './components/UnitConverter'
import { ErrorBoundary } from './components/ErrorBoundary'
import './App.css'

function App() {
  return (
    <ErrorBoundary>
      <div className="app">
        <UnitConverter />
      </div>
    </ErrorBoundary>
  )
}

export default App
