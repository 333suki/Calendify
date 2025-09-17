import { Outlet, Navigate } from "react-router-dom";

export default function ProtectedRoutes() {
    // TODO: Replace with authentication logic
    const authenticated = true;
    return authenticated ? <Outlet/> : <Navigate to="/" replace />;
}
