interface Room {
    id: number,
    name: string
}
//
// interface RoomBooking {
//     id: number,
//     roomID: number;
//     userID: number;
//     startTime: string;
//     endTime: string;
// }

import styles from "./SingleRoomBookingDisplay.module.css";

interface Props {
    bookingId: number;
    room: Room;
    bookingStartTime: string;
    bookingEndTime: string;
    getBookings(): Promise<void>;
    handleTokenRefresh: () => Promise<Response | null>;
}

export default function SingleRoomBookingDisplay({bookingId, room, bookingStartTime, bookingEndTime, getBookings, handleTokenRefresh}: Props){
    const deleteBooking = async () => {
        console.log("deleteBooking");
        try {
            let response = await fetch(`http://localhost:5117/room-bookings/${bookingId}`, {
                method: "DELETE",
                headers: {
                    "Authorization": `${localStorage.getItem("accessToken")}`,
                }
            });
            if (response.status === 498) {
                const refreshResult = await handleTokenRefresh();
                if (refreshResult) {
                    response = await fetch(`http://localhost:5117/room-bookings/${bookingId}`, {
                        method: "DELETE",
                        headers: {
                            "Authorization": `${localStorage.getItem("accessToken")}`,
                        }
                    });
                }
            }
            await getBookings();
        } catch (e) {
            console.error(e);
        }
    };

    return (
        <div className={styles.mainContainer}>
            <h1 className={styles.roomName}>{room.name}</h1>
            <h2 className={styles.startTime}>{new Date(bookingStartTime).toLocaleString()}</h2>
            <h2 className={styles.endTime}>{new Date(bookingEndTime).toLocaleString()}</h2>
            <button className={styles.deleteButton} type="button" onClick={deleteBooking}>Delete</button>
        </div>
    );
}
