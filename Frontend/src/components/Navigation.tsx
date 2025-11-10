import { useNavigate } from "react-router-dom";
import styles from "./Navigation.module.css";

export default function Navigation() {
    const navigate = useNavigate();
    const handleLogout = () => {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        navigate("/");
    };

    return (
        <nav className={styles.navigation}>
            <div
                className={`${styles.menuItem} ${styles.home}`}
                onClick={() => navigate("/home")}
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
                onClick={handleLogout}
            >
                Logout
            </div>
        </nav>
    );
}
