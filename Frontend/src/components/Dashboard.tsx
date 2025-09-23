import Styles from "./Dashboard.module.css"

export default function Dashboard() {
    return (
        <div className={Styles.mainContainer}>
            <div className={Styles.dashboard}>
                <div className={Styles.eventButton}>
                </div>
            <div className={Styles.agendaView}>
            </div>
            <div className={Styles.selectionMenu}>
            </div>
            <div className={Styles.sideMenu}>
            </div>
            </div>
            <div className={Styles.weeklyCalendar}>
            </div>
        </div>
    );
};
