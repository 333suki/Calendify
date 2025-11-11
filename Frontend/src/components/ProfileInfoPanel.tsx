import styles from "./ProfileInfoPanel.module.css";
import React, { useState, useEffect } from "react";

export default function ProfileInfoPanel() {
    const [profile, setProfile] = useState({
        username: "johndoe",
        email: "johndoe@example.com"
    });
    const [originalProfile, setOriginalProfile] = useState(profile);

    // TODO: Uncomment when API endpoints for getting current user username and email exist
    // useEffect(() => {
    //     const fetchProfile = async () => {
    //         const res = await fetch("http://localhost:5117/");
    //         if (res.ok) {
    //             const data = await res.json();
    //             setProfile(data);
    //             setOriginalProfile(data);
    //         }
    //     };
    //     fetchProfile();
    // }, []);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setProfile((prev) => ({ ...prev, [name]: value }));
    };

    const handleSave = async (e: React.FormEvent) => {
        e.preventDefault();

        if (profile.username === originalProfile.username && profile.email === originalProfile.email) {
            alert("No changes detected.");
            return;
        }

        // TODO: Uncomment when API endpoints for updating current user username and email exist
        // const response = await fetch("http://localhost:5117/", {
        //     method: "PUT",
        //     headers: {
        //         "Content-Type": "application/json"
        //     },
        //     body: JSON.stringify(profile),
        // });
        //
        // if (response.ok) {
        //     alert("Profile updated.");
        //     setOriginalProfile(profile);
        // } else {
        //     alert("Update failed.");
        // }
    };

    return (
        <div className={styles.mainContainer}>
            <h1>Profile Information</h1>
            <form onSubmit={handleSave}>
                <div className={styles.row}>
                    <h2>Username</h2>
                    <input name="username" type="text" value={profile.username} onChange={handleChange} required autoComplete="off"/>
                </div>
                <div className={styles.row}>
                    <h2>Email</h2>
                    <input name="email" type="email" value={profile.email} onChange={handleChange} required autoComplete="off"/>
                </div>
                <button type="submit">Save Changes</button>
            </form>
        </div>
    );
}
