import styles from "./OfficeAttendancePanel.module.css"
import OfficeAttendanceDisplay from "./OfficeAttendanceDisplay.tsx";

interface Props {
    selectedDate: Date | null,
}

function getWeekDates(date: Date | null): Date[] {
    if (!date) {
        return [];
    }
    const day = date.getDay();
    const sundayOffset = (day + 7) % 7;

    const sunday = new Date(date);
    sunday.setDate(date.getDate() - sundayOffset);

    return Array.from({ length: 7 }, (_, i) => {
        const d = new Date(sunday);
        d.setDate(sunday.getDate() + i);
        return d;
    });
}

export default function EventsPanel({selectedDate}: Props) {
    const weekDates = getWeekDates(selectedDate);
    const months = ["Januari", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]

    return (
        <div className={styles.mainContainer}>
            {selectedDate &&
                <div className={styles.monthDisplay}>
                    {months[selectedDate?.getMonth()] + " " + selectedDate?.getFullYear()}
                </div>
            }
            <div className={styles.events}>
                {weekDates.map(d => (
                    <div key={d.toISOString()}>
                        <OfficeAttendanceDisplay date={d} isSelected={d.getDay() == selectedDate?.getDay()}/>
                    </div>
                ))}
            </div>
        </div>
    )
}
