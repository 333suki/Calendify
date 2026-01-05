import React, { useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";

import styles from "./Login.module.css";

import personIcon from "../../assets/person.svg";

export default function ResetPassword() {
    const [token, setToken] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [message, setMessage] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();

    // Get token from URL params if available
    React.useEffect(() => {
        const urlToken = searchParams.get("token");
        if (urlToken) {
            setToken(urlToken);
        }
    }, [searchParams]);

    const handleResetPassword = async (e: React.FormEvent) => {
        e.preventDefault();

        if (newPassword !== confirmPassword) {
            setErrorMessage("Passwords do not match");
            return;
        }

        try {
            const response = await fetch("http://localhost:5117/auth/reset-password", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    token: token,
                    newPassword: newPassword
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
                setMessage("Password reset successfully. You can now login.");
                setErrorMessage("");
                setTimeout(() => navigate("/login"), 2000);
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
                <form onSubmit={handleResetPassword}>
                    <div className={styles.personIcon}>
                        <img src={personIcon} alt="Person"></img>
                    </div>
                    <h1>Reset Password</h1>
                    <div className={styles.inputBox}>
                        <input type="text" placeholder="Reset Token" value={token} onChange={(e) => {setToken(e.target.value); setErrorMessage(""); setMessage("")}} required/>
                    </div>
                    <div className={styles.inputBox}>
                        <input type="password" placeholder="New Password" value={newPassword} onChange={(e) => {setNewPassword(e.target.value); setErrorMessage(""); setMessage("")}} required/>
                    </div>
                    <div className={styles.inputBox}>
                        <input type="password" placeholder="Confirm New Password" value={confirmPassword} onChange={(e) => {setConfirmPassword(e.target.value); setErrorMessage(""); setMessage("")}} required/>
                    </div>
                    <div className={styles.error}>
                        {errorMessage && (<p className={styles.errorMessage}>{errorMessage}</p>)}
                        {message && (<p className={styles.successMessage}>{message}</p>)}
                    </div>
                    <button type="submit">Reset Password</button>
                    <div className={styles.registerLink}>
                        <p>Remember your password? <a onClick={() => {navigate("/login")}}>Login</a></p>
                    </div>
                </form>
            </div>
        </div>
    );
};
