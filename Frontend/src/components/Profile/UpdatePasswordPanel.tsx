import styles from "./ProfileInfoPanel.module.css";
import React, { useState, useEffect } from "react";

export default function UpdatePasswordPanel() {
    const [currentPassword, setCurrentPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");

    const handlePasswordUpdate = async (e: React.FormEvent) => {
        e.preventDefault();

        if (newPassword !== confirmPassword) {
            alert("Passwords don't match");
            return;
        }

        // TODO: Uncomment when API endpoints for updating current user password exists
        // const response = await fetch("http://localhost:5117/", {
        //     method: "PUT",
        //     headers: {
        //         "Content-Type": "application/json"
        //     },
        //     body: JSON.stringify({
        //         currentPassword: currentPassword,
        //         newPassword: newPassword,
        //         confirmPassword: confirmPassword
        //     })
        // });
        //
        // if (response.ok) {
        //     alert("Password updated.");
        // } else {
        //     alert("Password update failed.");
        // }
    };

    return (
        <div className={styles.mainContainer}>
            <h1>Update Password</h1>
            <form onSubmit={handlePasswordUpdate}>
                <div className={styles.row}>
                    <h2>Current Password</h2>
                    <input name="currentPassword" type="password" value={currentPassword} onChange={(e) => {setCurrentPassword(e.target.value);}} required autoComplete="off"/>
                </div>
                <div className={styles.row}>
                    <h2>New Password</h2>
                    <input name="newPassword" type="password" value={newPassword} onChange={(e) => {setNewPassword(e.target.value);}} required autoComplete="off"/>
                </div>
                <div className={styles.row}>
                    <h2>Confirm Password</h2>
                    <input name="confirmPassword" type="password" value={confirmPassword} onChange={(e) => {setConfirmPassword(e.target.value);}} required autoComplete="off"/>
                </div>
                <button type="submit">Update Password</button>
            </form>
        </div>
    );
}
