import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

import styles from "./Register.module.css";

import personIcon from "../assets/person.svg";

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

        // try {
        //     const response = await fakeLoginApi(username, password);

        //     if (response.success) {
        //         // role might be undefined, so fallback to "guest"
        //         localStorage.setItem("role", response.role ?? "guest");

        //         if (response.role === "admin") {
        //             navigate("/dashboard");
        //         } else {
        //             setErrorMessage("Unauthorized user. Only admins can log in.");
        //         }
        //     } else {
        //         // message might be undefined, so fallback to generic error
        //         setErrorMessage(response.message ?? "Login failed");
        //     }
        // } catch (err) {
        //     setErrorMessage("Something went wrong. Try again.");
        // }
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
