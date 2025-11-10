import React from "react";
import { useNavigate } from "react-router-dom";
import styles from "./Profile.module.css";
import Navigation from "./Navigation";


export default function Profile() {

const navigate = useNavigate();
const handleLogout = () => {
    navigate("/");
};


    return (
        <div className={styles.mainContainer}>
            <Navigation onLogout={handleLogout} />

            <div className={styles.HomeContainer}>
            </div>
        </div>
    );
}
