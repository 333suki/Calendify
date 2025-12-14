import { useEffect, useState } from "react";
import { Outlet, Navigate } from "react-router-dom";

export default function ProtectedRoutes() {
    const [authorized, setAuthorized] = useState<boolean | null>(null);

    useEffect(() => {
        const checkAuth = async () => {
            try {
                let response = await fetch("http://localhost:5117/auth/authorize", {
                    method: "POST",
                    headers: {
                        "Authorization": `${localStorage.getItem("accessToken")}`,
                    }
                });
                if (response.status === 498) {
                    console.log("Got 498");
                    console.log("Doing refresh request");
                    response = await fetch("http://localhost:5117/auth/refresh", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json",
                            "Authorization": `${localStorage.getItem("accessToken")}`,
                        },
                        body: JSON.stringify({
                            "refreshToken": `${localStorage.getItem("refreshToken")}`
                        })
                    });
                    let data = null;
                    const text = await response.text();
                    if (text) {
                        try {
                            data = JSON.parse(text);
                        } catch (err) {
                            console.error("Failed to parse JSON:", err);
                        }
                    }
                    if (response.ok) {
                        console.log("Refresh request OK");
                        if (data) {
                            localStorage.setItem("accessToken", data.accessToken);
                            localStorage.setItem("refreshToken", data.refreshToken);
                        }
                        console.log("Updated accessToken and refreshToken");
                    }
                }
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
