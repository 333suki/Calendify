import styles from "./Events.module.css";
import Navigation from "../Navigation";
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

interface Event {
    id: number;
    title: string;
    description: string;
    date: string;
    type: number;
}

export default function Events() {
    const [events, setEvents] = useState<Event[]>([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        fetchFutureEvents();
    }, []);

    const fetchFutureEvents = async () => {
        try {
            // Fetch events for the next 30 days
            const today = new Date();
            const futureEvents: Event[] = [];
            
            for (let i = 0; i < 30; i++) {
                const date = new Date(today);
                date.setDate(today.getDate() + i);
                const dateString = formatDateYYYYMMDD(date);
                
                let response = await fetch(`http://localhost:5117/event?date=${dateString}`, {
                    method: "GET",
                    headers: {
                        "Authorization": `${localStorage.getItem("accessToken")}`,
                    }
                });

                if (response.status === 498) {
                    const refreshResult = await handleTokenRefresh();
                    if (refreshResult) {
                        response = await fetch(`http://localhost:5117/event?date=${dateString}`, {
                            method: "GET",
                            headers: {
                                "Authorization": `${localStorage.getItem("accessToken")}`,
                            }
                        });
                    } else {
                        continue;
                    }
                }

                if (response.ok) {
                    const dayEvents = await response.json();
                    futureEvents.push(...dayEvents);
                }
            }

            // Filter out past events and sort by date
            const now = new Date();
            const filtered = futureEvents
                .filter(event => new Date(event.date) > now)
                .sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());

            setEvents(filtered);
        } catch (error) {
            console.error("Error fetching events:", error);
        } finally {
            setLoading(false);
        }
    };

    const handleTokenRefresh = async () => {
        try {
            const response = await fetch("http://localhost:5117/auth/refresh", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `${localStorage.getItem("accessToken")}`,
                },
                body: JSON.stringify({
                    "refreshToken": `${localStorage.getItem("refreshToken")}`
                })
            });

            if (response.ok) {
                const data = await response.json();
                localStorage.setItem("accessToken", data.accessToken);
                localStorage.setItem("refreshToken", data.refreshToken);
                return response;
            }
        } catch (error) {
            console.error("Token refresh failed:", error);
        }
        return null;
    };

    const formatDateYYYYMMDD = (date: Date) => {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, "0");
        const day = String(date.getDate()).padStart(2, "0");
        return `${year}-${month}-${day}`;
    };

    const formatEventDate = (dateString: string) => {
        const date = new Date(dateString);
        return date.toLocaleDateString("en-US", {
            weekday: "long",
            year: "numeric",
            month: "long",
            day: "numeric"
        });
    };

    const formatEventTime = (dateString: string) => {
        const date = new Date(dateString);
        return date.toLocaleTimeString("en-GB", {
            hour: "2-digit",
            minute: "2-digit",
            hour12: false
        });
    };

    const handleEventClick = (eventId: number) => {
        navigate(`/events/${eventId}`);
    };

    return (
        <div className={styles.mainContainer}>
            <div className={styles.navigationContainer}>
                <Navigation/>
            </div>
            <div className={styles.content}>
                <div className={styles.eventsListContainer}>
                    <h1 className={styles.pageTitle}>Upcoming Events</h1>
                    
                    {loading ? (
                        <div className={styles.loadingMessage}>Loading events...</div>
                    ) : events.length === 0 ? (
                        <div className={styles.noEventsMessage}>No upcoming events</div>
                    ) : (
                        <div className={styles.eventsList}>
                            {events.map(event => (
                                <div 
                                    key={event.id} 
                                    className={styles.eventCard}
                                    onClick={() => handleEventClick(event.id)}
                                >
                                    <div className={styles.eventHeader}>
                                        <h2 className={styles.eventTitle}>{event.title}</h2>
                                        <span className={styles.eventType}>
                                            {event.type === 0 ? "Event" : "Meeting"}
                                        </span>
                                    </div>
                                    <p className={styles.eventDescription}>{event.description}</p>
                                    <div className={styles.eventDateTime}>
                                        <div className={styles.eventDate}>
                                            📅 {formatEventDate(event.date)}
                                        </div>
                                        <div className={styles.eventTime}>
                                            🕐 {formatEventTime(event.date)}
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}