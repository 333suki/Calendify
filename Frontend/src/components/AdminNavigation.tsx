import { useNavigate, useLocation } from "react-router-dom";
import styles from "./AdminNavigation.module.css";

export default function Navigation() {
    const navigate = useNavigate();
    const location = useLocation();

    const handleLogout = () => {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken")
        navigate("/");
    };

    return (
        <nav className={styles.navigation}>
            <div
                className={`${styles.menuItem} ${location.pathname === "/admin/events" ? styles.active : ""}`}
                onClick={() => navigate("/admin/events")}
            >
                Admin Events
            </div>
            <div
                className={`${styles.menuItem} ${location.pathname === "/admin/rooms" ? styles.active : ""}`}
                onClick={() => navigate("/admin/rooms")}
            >
                Admin Rooms
            </div>
            <div
                className={`${styles.menuItem}`}
                onClick={() => handleLogout()}
            >
                Logout
            </div>
            {/*
            <div
                className={`${styles.menuItem}  ${location.pathname === "/profile"? styles.active : ""}`}
                onClick={() => navigate("/profile")}
            >
                Profile
            </div> */}

            {/* <div
                className={`${styles.menuItem}  ${location.pathname === "/settings" ? styles.active : ""}`}
                onClick={() => navigate("/settings")}
            >
                Settings
            </div> */}
        </nav>
    );
}
