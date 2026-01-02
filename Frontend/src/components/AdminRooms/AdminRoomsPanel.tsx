import styles from "./AdminRoomsPanel.module.css";
import {useState} from "react";
import AdminCreateRoomPanel from "./AdminCreateRoomPanel.tsx";
import AdminManageRoomsPanel from "./AdminManageRoomsPanel.tsx";

interface Room {
    id: number,
    name: string
}

export default function AdminRoomsPanel() {
    const [rooms, setRooms] = useState<Room[]>([])

    const getRooms = async () => {
        try {
            let response = await fetch(`http://localhost:5117/room`, {
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
                    response = await fetch(`http://localhost:5117/room`, {
                        method: "GET",
                        headers: {
                            "Authorization": `${localStorage.getItem("accessToken")}`,
                        }
                    });
                }
            }

            setRooms(await response.json());
        } catch (e) {
            console.error(e);
        }
    };

    return (
        <div className={styles.mainContainer}>
            <AdminCreateRoomPanel getRooms={getRooms}/>
            <AdminManageRoomsPanel rooms={rooms} getRooms={getRooms}/>
        </div>
    );
}
