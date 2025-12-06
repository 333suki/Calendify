import Navigation from "../Navigation";

import styles from "./OfficeAttendance.module.css";


export default function OfficeAttendance() {
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
