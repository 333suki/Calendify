import styles from "./EventsDisplay.module.css"
import { useEffect, useState } from "react";
import SingleEventDisplay from "./SingleEventDisplay";


interface Event {
    id: number,
    type: number,
    title: string
    description: string
    date: string
}

interface Props {
    date: Date,
    isSelected: boolean,
}

function formatDateYYYYMMDD(date: Date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");

    return `${year}-${month}-${day}`;
}

export default function EventsDisplay({date, isSelected}: Props) {
    const [events, setEvents] = useState<Event[]>([])
    const days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]

    useEffect(() => {
        // fetch(`http://localhost:5117/event?date=${formatDateYYYYMMDD(date)}`, {
        //     method: "GET",
        //     headers: {
        //         "Authorization": `${localStorage.getItem("accessToken")}`,
        //     }
        // })
        //
        // .then((res) => {
        //     if (!res.ok) {
        //         console.error("Failed to fetch events")
        //         console.error(res.status)
        //     }
        //     return res.json();
        // })
        // .then((data: Event[]) => {
        //     setEvents(data);
        // });

        const getEvents = async () => {
            try {
                let response = await fetch(`http://localhost:5117/event?date=${formatDateYYYYMMDD(date)}`, {
                    method: "GET",
                    headers: {
                        "Authorization": `${localStorage.getItem("accessToken")}`,
                    }
                });
                if (response.status === 498) {
                    console.log("Got 498");
                    console.log("Doing refresh request");
                    response = await fetch("http://localhost:5117/auth/refresh", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json",
                            "Authorization": `${localStorage.getItem("accessToken")}`,
                        },
                        body: JSON.stringify({
                            "refreshToken": `${localStorage.getItem("refreshToken")}`
                        })
                    });
                    let data = null;
                    const text = await response.text();
                    if (text) {
                        try {
                            data = JSON.parse(text);
                        } catch (err) {
                            console.error("Failed to parse JSON:", err);
                        }
                    }
                    if (response.ok) {
                        console.log("Refresh request OK");
                        if (data) {
                            localStorage.setItem("accessToken", data.accessToken);
                            localStorage.setItem("refreshToken", data.refreshToken);
                        }
                        console.log("Updated accessToken and refreshToken");

                        // Try again
                        response = await fetch(`http://localhost:5117/event?date=${formatDateYYYYMMDD(date)}`, {
                            method: "GET",
                            headers: {
                                "Authorization": `${localStorage.getItem("accessToken")}`,
                            }
                        });
                    }
                }

                setEvents(await response.json());
            } catch (e) {
                console.error(e);
            }
        };

        getEvents();
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
            <div className={styles.singleEventDisplays}>
                {[...events]
                    .sort((a, b) => new Date(a.date) - new Date(b.date))
                    .map((event) => (
                        <SingleEventDisplay
                            key={event.id}
                            title={event.title}
                            time={new Date(event.date).toLocaleString("en-GB", {
                                hour: "2-digit",
                                minute: "2-digit",
                                hour12: false,
                            })}
                            isSelected={isSelected}
                        />
                    ))}
            </div>
        </div>
    )
}
