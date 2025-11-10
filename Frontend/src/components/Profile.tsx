import styles from "./Profile.module.css";
import Navigation from "./Navigation";

export default function Profile() {
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
