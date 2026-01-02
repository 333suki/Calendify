import styles from "./AdminManageSingleRoom.module.css";

interface Props {
    id: number,
    name: string
    getRooms: () => Promise<void>
}

export default function AdminManageSingleRoom({id, name, getRooms}: Props) {
    const deleteRoom = async () => {
        try {
            let response = await fetch(`http://localhost:5117/room/${id}`, {
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
                    response = await fetch(`http://localhost:5117/room/${id}`, {
                        method: "DELETE",
                        headers: {
                            "Authorization": `${localStorage.getItem("accessToken")}`,
                        }
                    });
                }
            }
            await getRooms();
        } catch (e) {
            console.error(e);
        }
    };

    return (
        <div className={styles.mainContainer}>
            <h1 className={styles.roomName}>{name}</h1>
            <button className={styles.deleteButton} type="button" onClick={deleteRoom}>Delete</button>
        </div>
    );
}
