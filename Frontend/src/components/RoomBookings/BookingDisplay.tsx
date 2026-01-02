import styles from "./BookingDisplay.module.css";
import React, { useState, useEffect } from "react";

interface RoomBooking {
    roomID: number;
    userID: number;
    startTime: string;
    endTime: string;
}

export default function BookingDisplay() {
    const [bookings, setBookings] = useState<RoomBooking[]>([]);

    useEffect(() => {
        const getBookings = async () => {
            try {
                let response = await fetch("http://localhost:5117/room-bookings", {
                    method: "GET",
                    headers: {
                        "Authorization": `${localStorage.getItem("accessToken")}`,
                    }
                });

                if (response.status === 498) {
                    console.log("Got 498 → refreshing token");

                    response = await fetch("http://localhost:5117/auth/refresh", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json",
                            "Authorization": `${localStorage.getItem("accessToken")}`,
                        },
                        body: JSON.stringify({
                            refreshToken: `${localStorage.getItem("refreshToken")}`
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

                    if (response.ok && data) {
                        localStorage.setItem("accessToken", data.accessToken);
                        localStorage.setItem("refreshToken", data.refreshToken);

                        response = await fetch("http://localhost:5117/room-bookings", {
                            method: "GET",
                            headers: {
                                "Authorization": `${localStorage.getItem("accessToken")}`,
                            }
                        });
                    }
                }

                const bookingData = await response.json();
                console.log("Room bookings:", bookingData);
                setBookings(bookingData);

            } catch (e) {
                console.error(e);
            }
        };

        getBookings();
    }, []);

    return (
        <div className={styles.mainContainer}>
            <h1>Your bookings</h1>

            {bookings.length === 0 && (
                <p className={styles.empty}>No bookings found</p>
            )}

            <ul className={styles.bookingList}>
                {bookings.map((booking, index) => (
                    <li key={index} className={styles.bookingItem}>
                        <div className={styles.room}>
                            Room ID: {booking.roomID}
                        </div>
                        <div className={styles.time}>
                            {new Date(booking.startTime).toLocaleString()} –{" "}
                            {new Date(booking.endTime).toLocaleString()}
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
}
