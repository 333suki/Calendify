import React from "react";
import Navigation from "./Navigation";

import styles from "./Calendar.module.css";
import { useNavigate } from "react-router-dom";


export default function Calendar() {

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
