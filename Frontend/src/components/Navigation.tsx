import { useNavigate, useLocation } from "react-router-dom";
import styles from "./Navigation.module.css";

export default function Navigation() {
    const navigate = useNavigate();
    const location = useLocation();

    const handleLogout = () => {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        navigate("/");
    };

    return (
        <nav className={styles.navigation}>
            <div
                className={`${styles.menuItem} ${location.pathname === "/home" ? styles.active : ""}`}
                onClick={() => navigate("/home")}
            >
                Home
            </div>

            <div
                className={`${styles.menuItem}  ${location.pathname === "/events"? styles.active : ""}`}
                onClick={() => navigate("/events")}
            >
                Events
            </div>

            <div
                className={`${styles.menuItem}  ${location.pathname === "/room-bookings" ? styles.active : ""}`}
                onClick={() => navigate("/room-bookings")}
            >
                Room Bookings
            </div>

            <div
                className={`${styles.menuItem}  ${location.pathname === "/office-attendance"? styles.active : ""}`}
                onClick={() => navigate("/office-attendance")}
            >
                Office Attendance
            </div>

            <div
                className={`${styles.menuItem}  ${location.pathname === "/profile"? styles.active : ""}`}
                onClick={() => navigate("/profile")}
            >
                Profile
            </div>

            <div
                className={`${styles.menuItem}  ${location.pathname === "/settings" ? styles.active : ""}`}
                onClick={() => navigate("/settings")}
            >
                Settings
            </div>

            <div
                className={`${styles.menuItem}`}
                onClick={handleLogout}
            >
                Logout
            </div>
        </nav>
    );
}
