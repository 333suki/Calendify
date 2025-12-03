import styles from "./SmallAgenda.module.css";

import { DayPicker } from "react-day-picker";
import "react-day-picker/style.css";

interface Props {
    selectedDate: Date,
    setSelectedDate
}

export default function SmallAgenda({selectedDate, setSelectedDate}: Props) {
    return (
        <div className={styles.mainContainer}>
            <div className={styles.pickerContainer}>
                <DayPicker
                    animate
                    mode="single"
                    selected={selectedDate}
                    onSelect={setSelectedDate}
                    onMonthChange={(date) => {
                        const today = new Date;
                        if (date.getFullYear() == today.getFullYear() && date.getMonth() == today.getMonth()) {
                            setSelectedDate(today);
                        } else {
                            // Wake up, its da
                            const firstOfDaMonth = new Date(date.getFullYear(), date.getMonth(), 1);
                            setSelectedDate(firstOfDaMonth);
                        }
                    }}
                    // footer={
                    //     selected ? `Selected: ${selected.toLocaleDateString()}` : null
                    // }
                />
            </div>
        </div>
    )
}
