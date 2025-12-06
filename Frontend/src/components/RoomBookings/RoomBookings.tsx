import styles from "./RoomBookings.module.css";
import Navigation from "../Navigation";
import RoomBookingPanel from "./RoomBookingPanel";

export default function RoomBookings() {
    return (
        <div className={styles.mainContainer}>
            <div className ={styles.navigationContainer}>
                <Navigation/>
            </div>
            <div className={styles.content}>
                <RoomBookingPanel/>
            </div>
        </div>
    );
}