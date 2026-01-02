import styles from "./AdminManageRoomsPanel.module.css";
import {useEffect} from "react";
import AdminManageSingleRoom from "./AdminManageSingleRoom.tsx";

interface Room {
    id: number,
    name: string
}

interface Props {
    rooms: Room[]
    getRooms: () => Promise<void>
}

export default function AdminManageRoomsPanel({rooms, getRooms}: Props) {
    useEffect(() => {
        getRooms();
    }, []);

    return (
        <div className={styles.mainContainer}>
            <h1 className={styles.pageTitle}>Manage Rooms</h1>
            <div className={styles.roomsDisplay}>
                {[...rooms]
                    // .sort((a, b) => a.id - b.id)
                    .map((room) => (
                        <AdminManageSingleRoom
                            key={room.id}
                            id={room.id}
                            name={room.name}
                            getRooms={getRooms}
                        />
                    ))}
            </div>
            <button className={styles.refreshButton} type="button" onClick={getRooms}>
                Refresh
            </button>
        </div>
    );
}
