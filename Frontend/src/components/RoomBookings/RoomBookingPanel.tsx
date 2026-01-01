import styles from "./RoomBookingPanel.module.css";
import RoomInfoPanel from "./RoomInfoPanel";

export default function RoomBookingPanel() {
    return (
        <div className={styles.mainContainer}>
            <RoomInfoPanel/>
        </div>
    );
}
