import styles from "./EventsDisplay.module.css"
import { useEffect, useState } from "react";
import SingleEventDisplay from "./SingleEventDisplay";


interface Event {
  id: number
  title: string
  description: string
  date: string
}

interface Props {
    date: Date,
    isSelected: boolean,
}

function formatDateYYYYMMDD(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");

    return `${year}-${month}-${day}`;
}

export default function EventsDisplay({date, isSelected, setSelectedDate}: Props) {
    const [events, setEvents] = useState<Event[]>([])
    const days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]

    useEffect(() => {
        console.log(date)
        fetch(`http://localhost:5117/event?date=${formatDateYYYYMMDD(date)}`, {
            method: "GET",
            headers: {
                "Authorization": `${localStorage.getItem("accessToken")}`,
            }
        })

        .then((res) => {
            if (!res.ok) {
                console.error("Failed to fetch events")
                console.error(res.status)
            }
            return res.json();
        })
        .then((data: Event[]) => {
            setEvents(data);
        });
    }, []);

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
                {events.map((event) => 
                <SingleEventDisplay key={event.id} title={event.title} time={event.date}/>
            )}
        </div>
    )
}
