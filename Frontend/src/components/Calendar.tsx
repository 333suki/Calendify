import Navigation from "./Navigation";

import styles from "./Calendar.module.css";

export default function Calendar() {
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
