import styles from "./Profile.module.css";
import Navigation from "../Navigation";
import ProfilePanel from "./ProfilePanel.tsx";

export default function Profile() {
    return (
        <div className={styles.mainContainer}>
            <div className ={styles.navigationContainer}>
                <Navigation/>
            </div>
            <div className={styles.content}>
                <ProfilePanel />
            </div>

        </div>
    );
}
