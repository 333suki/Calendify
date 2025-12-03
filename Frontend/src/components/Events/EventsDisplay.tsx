import styles from "./EventsDisplay.module.css"

interface Props {
    date: Date,
    isSelected: boolean,
}

export default function EventsDisplay({date, isSelected, setSelectedDate}: Props) {
    const days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]
    return (
        <div className={`${styles.mainContainer} ${isSelected ? styles.selected : ""}`}>
            <div className={styles.dateText}>
                <div className={styles.day}>
                    {days[date.getDay()]}
                </div>
                <div className={styles.date}>
                    {date.getDate()}
                </div>
            </div>
        </div>
    )
}
