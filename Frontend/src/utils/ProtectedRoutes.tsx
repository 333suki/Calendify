import { useEffect, useState } from "react";
import { Outlet, Navigate } from "react-router-dom";

export default function ProtectedRoutes() {
    const [authorized, setAuthorized] = useState<boolean | null>(null);

    useEffect(() => {
        const checkAuth = async () => {
            try {
                const response = await fetch("http://localhost:5117/auth/authorize", {
                    method: "POST",
                    credentials: "include",
                });
                setAuthorized(response.ok);
            } catch {
                setAuthorized(false);
            }
        };

        checkAuth();
    }, []);

    if (authorized === null) return;

    return authorized ? <Outlet /> : <Navigate to="/" replace />;
}
