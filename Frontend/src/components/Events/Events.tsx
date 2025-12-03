import styles from "./Events.module.css";
import SmallAgenda from "../SmallAgenda";
import Navigation from "../Navigation";
import { useState } from 'react'
import EventsPanel from "./EventsPanel";

export default function Events() {
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
                {/* {selectedDate.toDateString()} */}
                <EventsPanel selectedDate={selectedDate}/>
            </div>
        </div>
    );
}
