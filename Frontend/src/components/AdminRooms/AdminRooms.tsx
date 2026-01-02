import styles from "./AdminRooms.module.css";
import AdminNavigation from "../AdminNavigation.tsx";
import AdminRoomsPanel from "./AdminRoomsPanel.tsx";

export default function AdminRooms() {
    return (
        <div className={styles.mainContainer}>
            <div className ={styles.navigationContainer}>
                <AdminNavigation/>
            </div>
            <div className={styles.content}>
                <AdminRoomsPanel/>
            </div>
        </div>
    );
}
