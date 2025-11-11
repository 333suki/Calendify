import styles from "./ProfilePanel.module.css";
import ProfileInfoPanel from "./ProfileInfoPanel.tsx";
import UpdatePasswordPanel from "./UpdatePasswordPanel.tsx";

export default function ProfilePanel() {
    return (
        <div className={styles.mainContainer}>
            <ProfileInfoPanel/>
            <UpdatePasswordPanel/>
        </div>
    );
}
