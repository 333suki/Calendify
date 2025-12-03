import styles from "./RoomBookings.module.css";
import Navigation from "../Navigation";


export default function RoomBookings() {
    return (
        <div className={styles.mainContainer}>
            <div className ={styles.navigationContainer}>
                <Navigation/>
            </div>
            <div className={styles.content}>

            </div>
        </div>
    );
}
