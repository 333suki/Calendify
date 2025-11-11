import { useNavigate, useLocation } from "react-router-dom";
import styles from "./ProfilePanel.module.css";
import React from "react";
import ProfileInfo from "./ProfileInfo";

export default function ProfilePanel() {

    return (
        <div className={styles.mainContainer}>
            <ProfileInfo />
        </div>
    );
}
