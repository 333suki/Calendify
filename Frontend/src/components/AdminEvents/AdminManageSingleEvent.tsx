import styles from "./AdminManageSingleEvent.module.css";

interface Props {
    id: number,
    type: EventType,
    title: string
    description: string
    date: string,
    getEvents: () => Promise<void>,
}

enum EventType {
    Event = 0,
    Meeting = 1,
    Birthday = 2,
    Holiday = 3,
    Training = 4,
    Social = 5,
    Incident = 6
}

export default function AdminManageSingleEvent({id, type, title, description, date, getEvents}: Props) {
    const deleteEvent = async () => {
        try {
            let response = await fetch(`http://localhost:5117/event/${id}`, {
                method: "DELETE",
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
                    response = await fetch(`http://localhost:5117/event/${id}`, {
                        method: "DELETE",
                        headers: {
                            "Authorization": `${localStorage.getItem("accessToken")}`,
                        }
                    });
                }
            }
            await getEvents();
        } catch (e) {
            console.error(e);
        }
    };

    return (
        <div className={styles.mainContainer}>
            <h2 className={styles.eventType}>{EventType[type]}</h2>
            <h1 className={styles.eventTitle}>{title}</h1>
            <h2 className={styles.eventDescription}>{description}</h2>
            <h2 className={styles.eventDate}>{date}</h2>
            <button className={styles.deleteButton} type="button" onClick={deleteEvent}>Delete</button>
        </div>
    );
}
