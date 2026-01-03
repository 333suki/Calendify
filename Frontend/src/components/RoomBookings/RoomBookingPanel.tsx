import styles from "./RoomBookingPanel.module.css";
import BookingDisplay from "./BookingDisplay";
import CreateBooking from "./CreateBooking";
import {useEffect, useState} from "react";

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

export default function RoomBookingPanel() {
    const [rooms, setRooms] = useState<Room[]>([]);
    const [bookings, setBookings] = useState<RoomBooking[]>([]);

    const handleTokenRefresh = async () => {
        try {
            const response = await fetch("http://localhost:5117/auth/refresh", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `${localStorage.getItem("accessToken")}`
                },
                body: JSON.stringify({
                    refreshToken: `${localStorage.getItem("refreshToken")}`
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

    const getRooms = async () => {
        try {
            let response = await fetch("http://localhost:5117/room", {
                headers: {
                    "Authorization": `${localStorage.getItem("accessToken")}`
                }
            });

            if (response.status === 498) {
                const refreshResult = await handleTokenRefresh();
                if (refreshResult) {
                    response = await fetch("http://localhost:5117/room", {
                        headers: {
                            "Authorization": `${localStorage.getItem("accessToken")}`
                        }
                    });
                }
            }

            if (response.ok) {
                const data = await response.json();
                setRooms(data);
            }
        } catch (error) {
            console.error("Error fetching rooms:", error);
        }
    };

    const getBookings = async () => {
        try {
            let response = await fetch("http://localhost:5117/room-bookings", {
                method: "GET",
                headers: {
                    "Authorization": `${localStorage.getItem("accessToken")}`,
                }
            });

            if (response.status === 498) {
                const refreshResult = await handleTokenRefresh();
                if (refreshResult) {
                    response = await fetch("http://localhost:5117/room-bookings", {
                        method: "GET",
                        headers: {
                            "Authorization": `${localStorage.getItem("accessToken")}`,
                        }
                    });
                }
            }

            const bookingData = await response.json();
            setBookings(bookingData);
        } catch (e) {
            console.error(e);
        }
    };

    useEffect(() => {
        getRooms();
    }, []);

    return (
        <div className={styles.mainContainer}>
            <CreateBooking rooms={rooms} setRooms={setRooms} getRooms={getRooms} handleTokenRefresh={handleTokenRefresh} getBookings={getBookings}/>
            <BookingDisplay rooms={rooms} setRooms={setRooms} getRooms={getRooms} handleTokenRefresh={handleTokenRefresh} bookings={bookings} setBookings={setBookings} getBookings={getBookings}/>
        </div>
    );
}
