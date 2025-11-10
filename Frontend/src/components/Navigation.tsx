import React from "react";
import { useNavigate } from "react-router-dom";
import styles from "./Navigation.module.css";

interface NavigationProps {
    onLogout: () => void;
}

export default function Navigation({ onLogout }: NavigationProps) {
    const navigate = useNavigate();

    return (
        <nav className={styles.navigation}>
            <div
                className={`${styles.menuItem} ${styles.home}`}
                onClick={() => navigate("/dashboard")}
            >
                Home
            </div>

            <div
                className={`${styles.menuItem} ${styles.calendar}`}
                onClick={() => navigate("/calendar")}
            >
                Calendar
            </div>

            <div
                className={`${styles.menuItem} ${styles.events}`}
                onClick={() => navigate("/events")}
            >
                Events

            </div>

            <div
                className={`${styles.menuItem} ${styles.events}`}
                onClick={() => navigate("/room-bookings")}
            >
                Room Bookings
            </div>

            <div
                className={`${styles.menuItem} ${styles.events}`}
                onClick={() => navigate("/office-attendance")}
            >
                Office Attendance
            </div>


            <div
                className={`${styles.menuItem} ${styles.profile}`}
                onClick={() => navigate("/profile")}
            >
                Profile
            </div>

            <div
                className={`${styles.menuItem} ${styles.settings}`}
                onClick={() => navigate("/settings")}
            >
                Settings
            </div>

            <div
                className={`${styles.menuItem} ${styles.logout}`}
                onClick={onLogout}
            >
                Logout
            </div>
        </nav>
    );
}
