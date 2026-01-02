import styles from "./AdminEventsPanel.module.css";
import AdminCreateEventPanel from "./AdminCreateEventPanel.tsx";
import AdminManageEventsPanel from "./AdminManageEventsPanel.tsx";
import {useState} from "react";

interface Event {
    id: number,
    type: number,
    title: string
    description: string
    date: string
}

export default function AdminEventsPanel() {
    const getEvents = async () => {
        try {
            let response = await fetch(`http://localhost:5117/event`, {
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
                    response = await fetch(`http://localhost:5117/event`, {
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

    const [events, setEvents] = useState<Event[]>([])
    return (
        <div className={styles.mainContainer}>
            <AdminCreateEventPanel getEvents={getEvents}/>
            <AdminManageEventsPanel events={events} getEvents={getEvents}/>
        </div>
    );
}
