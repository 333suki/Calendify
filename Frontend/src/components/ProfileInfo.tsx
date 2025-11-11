import { useNavigate, useLocation } from "react-router-dom";
import styles from "./ProfileInfo.module.css";
import React from "react";


export default function ProfileInfo() {

    return (
        <div className={styles.mainContainer}>
            <h2>Profile Info</h2>
            <p>Username: johndoe</p>
            <p>Email: johndoe@hotmail.com </p>
        </div>
    );
}
