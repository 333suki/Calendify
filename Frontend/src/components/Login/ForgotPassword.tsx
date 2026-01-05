import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

import styles from "./Login.module.css";

import personIcon from "../../assets/person.svg";

export default function ForgotPassword() {
    const [email, setEmail] = useState("");
    const [message, setMessage] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();

    const handleForgotPassword = async (e: React.FormEvent) => {
        e.preventDefault();

        try {
            const response = await fetch("http://localhost:5117/auth/forgot-password", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    email: email
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
                setMessage("Password reset email sent successfully. Check your email for the reset token.");
                setErrorMessage("");
            } else {
                setErrorMessage(data?.message || "Request failed");
            }
        } catch (err) {
            setErrorMessage(`Something went wrong. ${err}`);
        }
    };

    return (
        <div className={styles.mainContainer}>
            <div className={styles.loginContainer}>
                <form onSubmit={handleForgotPassword}>
                    <div className={styles.personIcon}>
                        <img src={personIcon} alt="Person"></img>
                    </div>
                    <h1>Reset Password</h1>
                    <div className={styles.inputBox}>
                        <input type="email" placeholder="Email" value={email} onChange={(e) => {setEmail(e.target.value); setErrorMessage(""); setMessage("")}} required/>
                    </div>
                    <div className={styles.error}>
                        {errorMessage && (<p className={styles.errorMessage}>{errorMessage}</p>)}
                        {message && (<p className={styles.successMessage}>{message}</p>)}
                    </div>
                    <button type="submit">Send Reset Email</button>
                    <div className={styles.registerLink}>
                        <p>Remember your password? <a onClick={() => {navigate("/")}}>Login</a></p>
                    </div>
                </form>
            </div>
        </div>
    );
};
