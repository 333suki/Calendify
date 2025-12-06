import styles from "./Home.module.css";
import SmallAgenda from "../SmallAgenda";
import Navigation from "../Navigation";
import { useState } from 'react'

export default function Home() {
    // const [selectedDate, setSelectedDate] = useState<Date>(new Date());
    return (
        <div className={styles.mainContainer}>
            <div className ={styles.navigationContainer}>
                <Navigation/>
            </div>
            <div className={styles.content}>
                {/* <SmallAgenda 
                    selectedDate={selectedDate}
                    setSelectedDate={setSelectedDate}
                /> */}
                {/* {selectedDate?.toDateString()} */}
            </div>
        </div>
    );
}
