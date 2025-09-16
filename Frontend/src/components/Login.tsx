import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

import styles from "./Login.module.css";

import calendarIcon from "../assets/calendar.svg";

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
                <img className={styles.personIcon} src={personIcon} alt="Person"></img>
                <h1 className={styles.title}>User Login</h1>
                <form onSubmit={handleLogin} className={styles.form}>
                    <input type= "text" id={styles.username} placeholder="Username"></input>
                    <input type= "password" id={styles.password} placeholder="Password"></input>
                    <button type="submit" className={styles.loginButton}>Login</button>
                </form>
                <a href="https://google.com"className={styles.forgotPassword}>Forgot password</a>
                <a href="https://google.com" className={styles.signUp}>Sign Up</a>
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
