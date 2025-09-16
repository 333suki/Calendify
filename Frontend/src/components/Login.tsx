import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

import styles from "./Login.module.css";

import personIcon from "../assets/person.svg";

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
        <div className={styles.mainContainer}>
            <div className={styles.loginContainer}>
                <form onSubmit={handleLogin}>
                    <div className={styles.personIcon}>
                        <img src={personIcon} alt="Person"></img>
                    </div>
                    <h1>Login</h1>
                    <div className={styles.inputBox}>
                        <input type="text" placeholder="Username" value={username} onChange={(e) => {setUsername(e.target.value); setErrorMessage("")}} required/>
                    </div>
                    <div className={styles.inputBox}>
                        <input type="password" placeholder="Password" value={password} onChange={(e) => {setPassword(e.target.value); setErrorMessage("")}} required/>
                    </div>
                    <div className={styles.errorForgot}>
                        <div className={styles.error}>
                            {errorMessage && (<p className={styles.errorMessage}>{errorMessage}</p>)}
                        </div>
                        <div className={styles.forgot}>
                            <a href="https://google.com">Forgot password</a>
                        </div>
                    </div>
                    <button type="submit">Login</button>
                    <div className={styles.signUpLink}>
                        <p>Dont have an account? <a href="https://google.com">Sign Up</a></p>
                    </div>
                </form>
            </div>
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
