import styles from "./BookingDisplay.module.css";
import { useEffect } from "react";
import SingleRoomBookingDisplay from "./SingleRoomBookingDisplay.tsx";

interface Room {
    id: number,
    name: string
}

interface RoomBooking {
    id: number;
    roomID: number;
    userID: number;
    startTime: string;
    endTime: string;
}

interface Props {
    rooms: Room[]
    setRooms: (rooms: Room[]) => void
    getRooms(): Promise<void>
    handleTokenRefresh: () => Promise<Response | null>
    bookings: RoomBooking[]
    setBookings: (bookings: RoomBooking[]) => void
    getBookings(): Promise<void>
}

export default function BookingDisplay({rooms, handleTokenRefresh, bookings, getBookings}: Props) {
    useEffect(() => {
        getBookings();
    }, []);

    return (
        <div className={styles.mainContainer}>
            <h1 className={styles.panelTitle}>Your bookings</h1>

            {bookings.length === 0 && (
                <p className={styles.empty}>No bookings found</p>
            )}

            <div className={styles.bookingList}>
                {[...bookings]
                    .sort((a, b) => new Date(a.startTime).getTime() - new Date(b.startTime).getTime())
                    .map((booking) => (
                        <SingleRoomBookingDisplay
                            key={booking.id}
                            bookingId={booking.id}
                            room={rooms.find((room) => room.id == booking.roomID)}
                            bookingStartTime={booking.startTime}
                            bookingEndTime={booking.endTime}
                            getBookings={getBookings}
                            handleTokenRefresh={handleTokenRefresh}
                        />
                    ))}
            </div>
        </div>
    );
}
