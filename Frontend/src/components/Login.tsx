import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

import styles from "./Login.module.css";

const Login: React.FC = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();

        try {
            const response = await fakeLoginApi(username, password);

            if (response.success) {
                // role might be undefined, so fallback to "guest"
                localStorage.setItem("role", response.role ?? "guest");

                if (response.role === "admin") {
                    navigate("/dashboard");
                } else {
                    setErrorMessage("Unauthorized user. Only admins can log in.");
                }
            } else {
                // message might be undefined, so fallback to generic error
                setErrorMessage(response.message ?? "Login failed");
            }
        } catch (err) {
            setErrorMessage("Something went wrong. Try again.");
        }
    };


    return (
        <div>
            <form onSubmit={handleLogin}>
                <h1 className={styles.loginText}>Login</h1>
                {errorMessage && (<p>{errorMessage}</p>)}
                <input
                    type="text"
                    placeholder="Username"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    required
                />
                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
                <button type="submit">Login</button>
            </form>
        </div>
    );
};

// Mock API function
async function fakeLoginApi(username: string, password: string) {
    return new Promise<{ success: boolean; role?: string; message?: string }>(
        (resolve) => {
            setTimeout(() => {
                if (username === "admin" && password === "admin123") {
                    resolve({ success: true, role: "admin" });
                } else {
                    resolve({ success: false, message: "Password is incorrect" });
                }
            }, 1000);
        }
    );
}

export default Login;
