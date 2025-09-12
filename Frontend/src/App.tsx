import {BrowserRouter as Router, Routes, Route} from "react-router-dom";

import Login from "./components/Login.tsx"
import Dashboard from "./components/Dashboard.tsx";

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<Login/>}/>
                <Route path="/dashboard" element={<Dashboard />} />
            </Routes>
        </Router>
    )
}

export default App
