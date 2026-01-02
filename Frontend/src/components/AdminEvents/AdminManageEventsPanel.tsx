import styles from "./AdminManageEventsPanel.module.css";
import {useEffect} from "react";
import AdminManageSingleEvent from "./AdminManageSingleEvent";

interface Event {
    id: number,
    type: number,
    title: string
    description: string
    date: string
}

interface Props {
    events: Event[]
    getEvents: () => Promise<void>
}

export default function AdminManageEventsPanel({events, getEvents}: Props) {
    useEffect(() => {
        getEvents();
    }, []);

    return (
        <div className={styles.mainContainer}>
            <h1 className={styles.pageTitle}>Manage Events</h1>
            <div className={styles.eventsDisplay}>
                {[...events]
                    .sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime())
                    .map((event) => (
                        <AdminManageSingleEvent
                            key={event.id}
                            id={event.id}
                            type={event.type}
                            title={event.title}
                            description={event.description}
                            date={event.date}
                            getEvents={getEvents}
                        />
                    ))}
            </div>
            <button className={styles.refreshButton} type="button" onClick={getEvents}>
                Refresh
            </button>
        </div>
    );
}
