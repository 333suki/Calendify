import styles from "./Settings.module.css";
import Navigation from "../Navigation";


export default function Settings() {
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
