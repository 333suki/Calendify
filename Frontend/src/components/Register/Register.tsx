import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

import styles from "./Register.module.css";

import personIcon from "../../assets/person.svg";

export default function Register() {
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("")
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();

        if (password !== confirmPassword) {
            setErrorMessage("Passwords do not match.");
            return;
        }
        
        try {
            const response = await fetch("http://localhost:5117/auth/register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    username: username,
                    email: email,
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
                navigate("/")
            } else {

                if (response.status === 404 || response.status === 400) {
                    setErrorMessage(data?.message);
                } else if(response.status === 409) {
                    setErrorMessage("Username already taken")
                }
                else {
                    setErrorMessage(`Unexpected error: ${response.status}`);
                }
            }

        } catch (err) {
            setErrorMessage(`Something went wrong. ${err}`);
        }
    };

    return (
        <div className={styles.mainContainer}>
            <div className={styles.registerContainer}>
                <form onSubmit={handleRegister}>
                    <div className={styles.personIcon}>
                        <img src={personIcon} alt="Person"></img>
                    </div>
                    <h1>Register</h1>
                    <div className={styles.inputBox}>
                        <input type="text" placeholder="Username" value={username} onChange={(e) => {setUsername(e.target.value); setErrorMessage("")}} required/>
                    </div>
                    <div className={styles.inputBox}>
                        <input 
                        type="email" placeholder="Email" value={email} onChange={(e) => {setEmail(e.target.value); setErrorMessage("")}} required/>
                    </div>
                    <div className={styles.inputBox}>
                        <input type="password" placeholder="Password" value={password} onChange={(e) => {setPassword(e.target.value); setErrorMessage("")}} required/>
                    </div>
                    <div className={styles.inputBox}>
                        <input type="password" placeholder="Confirm Password" value={confirmPassword} onChange={(e) => {setConfirmPassword(e.target.value); setErrorMessage("")}} required/>
                    </div>
                    <div className={styles.errorBox}>
                        <div className={styles.error}>
                            {errorMessage && (<p className={styles.errorMessage}>{errorMessage}</p>)}
                        </div>
                    </div>
                    <button type="submit">Register</button>
                    <div className={styles.signUpLink}>
                        <p>Already have an account? <a onClick={() => {navigate("/")}}>Log In</a></p>
                    </div>
                </form>
            </div>
        </div>
    );
};
