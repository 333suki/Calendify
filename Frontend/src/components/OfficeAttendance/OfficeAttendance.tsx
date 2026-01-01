import Navigation from "../Navigation";
import styles from "./OfficeAttendance.module.css";
import SmallAgenda from "../SmallAgenda.tsx";
import {useState} from "react";
import OfficeAttendancePanel from "./OfficeAttendancePanel.tsx";

export default function OfficeAttendance() {
    const [selectedDate, setSelectedDate] = useState<Date>(new Date());

    return (
        <div className={styles.mainContainer}>
            <div className ={styles.navigationContainer}>
                <Navigation/>
            </div>
            <div className={styles.content}>
                <SmallAgenda
                    selectedDate={selectedDate}
                    setSelectedDate={setSelectedDate}
                />
                <OfficeAttendancePanel selectedDate={selectedDate}/>
            </div>
        </div>
    );
}
