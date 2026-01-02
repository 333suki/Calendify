import styles from "./AdminEvents.module.css";
import AdminNavigation from "../AdminNavigation.tsx";
import AdminEventsPanel from "./AdminEventsPanel.tsx";

export default function RoomBookings() {
    return (
        <div className={styles.mainContainer}>
            <div className ={styles.navigationContainer}>
                <AdminNavigation/>
            </div>
            <div className={styles.content}>
                <AdminEventsPanel/>
            </div>
        </div>
    );
}