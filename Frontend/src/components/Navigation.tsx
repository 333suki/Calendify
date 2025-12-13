import { useNavigate, useLocation } from "react-router-dom";
import styles from "./Navigation.module.css";
import { useState } from "react";

export default function Navigation() {
    const navigate = useNavigate();
    const location = useLocation();
    const [accountOpen, setAccountOpen] = useState(false);

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

            <div className={styles.menuItem} style={{ position: "relative" }}>
                <button
                    onClick={() => setAccountOpen((v) => !v)}
                    style={{
                        background: "transparent",
                        border: "transparent",
                        color: "white"}}
                        
                >
                    Account
                </button>

                {accountOpen && (
                    <div
                        role="menu"
                        style={{
                            position: "absolute",
                            top: "100%",
                            left: 0,
                            background: "#3a475864",
                            // border: "1px solid #ccc",
                            boxShadow: "0 4px 8px rgba(0,0,0,0.08)",
                            zIndex: 20,
                            minWidth: 140,
                        }}
                    >
                        <div
                            role="menuitem"
                            className={styles.menuItem}
                            onClick={() => {
                                setAccountOpen(false);
                                navigate("/profile");
                            }}
                        >
                            Profile
                        </div>
                        <div
                            role="menuitem"
                            className={styles.menuItem}
                            onClick={() => {
                                setAccountOpen(false);
                                navigate("/settings");
                            }}
                        >
                            Settings
                        </div>
                        <div
                            role="menuitem"
                            className={styles.menuItem}
                            onClick={() => {
                                setAccountOpen(false);
                                handleLogout();
                            }}
                        >
                            Logout
                        </div>
                    </div>
                )}
            </div>

        </nav>
    );
}
