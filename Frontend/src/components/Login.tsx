import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

import styles from "./Login.module.css";

import personIcon from "../assets/person.svg";

export default function Login() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();

        try {
            const response = await fakeLoginApi(username, password);
            if (response.success) {
                // localStorage.setItem("role", response.role ?? "guest");
                navigate("/dashboard");
            } else {
                setErrorMessage(response.message ?? "Login failed.");
            }
        } catch (err) {
            setErrorMessage(`Something went wrong. ${err}`);
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
                    <div className={styles.registerLink}>
                         <p>Dont have an account? <a onClick={() => {navigate("/register")}}>Register</a></p>
                    </div>
                </form>
            </div>
        </div>
    );
};

// Mock API function
async function fakeLoginApi(username: string, password: string) {
    return new Promise<{ success: boolean; message?: string }>(
        (resolve) => {
            if (username === "admin" && password === "admin123") {
                resolve({ success: true });
            } else {
                resolve({ success: false, message: "Password is incorrect" });
            }
        }
    );
}
