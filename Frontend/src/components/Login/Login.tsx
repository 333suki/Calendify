import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

import styles from "./Login.module.css";

import personIcon from "../../assets/person.svg";

const ROLE = {
    ADMIN: 0,
    USER: 1
}

export default function Login() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();

        try {
            const response = await fetch("http://localhost:5117/auth/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    username: username,
                    password: password
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
                if (data) {
                    localStorage.setItem("accessToken", data.accessToken);
                    localStorage.setItem("refreshToken", data.refreshToken);
                    if (data.role == ROLE.USER) {
                        navigate("/home");
                    } else if (data.role == ROLE.ADMIN) {
                        navigate("/admin/events");
                    }
                }
            } else {
                if (response.status === 404 || response.status === 400) {
                    setErrorMessage(data?.message || "Request failed");
                } else if (response.status === 401) {
                    setErrorMessage("Invalid password");
                } else {
                    setErrorMessage(`Unexpected error: ${response.status}`);
                }
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
