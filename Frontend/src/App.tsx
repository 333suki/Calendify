import { BrowserRouter, Routes, Route } from "react-router-dom";

import Dashboard from "./components/Dashboard.tsx";
import Login from "./components/Login.tsx"
import ProtectedRoutes from "./utils/ProtectedRoutes.tsx";
import Register from "./components/Register.tsx"

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Login/>}/>
                <Route path="/register" element={<Register/>}/>
                <Route element={<ProtectedRoutes/>}>
                    <Route path="/dashboard" element={<Dashboard/>} />
                </Route>
            </Routes>
        </BrowserRouter>
    )
}

export default App
