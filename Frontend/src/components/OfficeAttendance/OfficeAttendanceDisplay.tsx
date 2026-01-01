import styles from "./OfficeAttendanceDisplay.module.css";
import SingleOfficeAttendanceDisplay from "./SingleOfficeAttendanceDisplay.tsx";

interface Props {
    date: Date,
    isSelected: boolean,
}

export default function OfficeAttendanceDisplay({date, isSelected}: Props) {
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
            <div className={styles.singleAttendanceDisplays}>
                <SingleOfficeAttendanceDisplay date={date} isSelected={isSelected}/>
                {/*{[...events]*/}
                {/*    .sort((a, b) => new Date(a.date) - new Date(b.date))*/}
                {/*    .map((event) => (*/}
                {/*        <SingleEventDisplay*/}
                {/*            key={event.id}*/}
                {/*            title={event.title}*/}
                {/*            time={new Date(event.date).toLocaleString("en-GB", {*/}
                {/*                hour: "2-digit",*/}
                {/*                minute: "2-digit",*/}
                {/*                hour12: false,*/}
                {/*            })}*/}
                {/*            isSelected={isSelected}*/}
                {/*        />*/}
                {/*    ))}*/}
            </div>
        </div>
    )
}
