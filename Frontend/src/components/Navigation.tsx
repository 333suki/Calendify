import React from "react";
import styles from "./Navigation.module.css";

interface NavigationProps {
    onLogout: () => void;
}

export default function Navigation({ onLogout }: NavigationProps) {
    return (
        <div className={styles.sideMenu}>
            <h2>Menu</h2>
            <div className={`${styles.menuItem} ${styles.profile}`} onClick={() => alert('Profile clicked')}>
                Profile
            </div>
            <div className={`${styles.menuItem} ${styles.settings}`} onClick={() => alert('Settings clicked')}>
                Settings
            </div>
            <div className={`${styles.menuItem} ${styles.logout}`} onClick={onLogout}>
                Logout
            </div>
        </div>
    );
}
