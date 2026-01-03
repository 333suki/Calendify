import React, { useState } from "react";
import styles from "./CreateBooking.module.css";

interface Room{
    id: number,
    name: string
}

interface Props {
    rooms: Room[]
    setRooms: (rooms: Room[]) => void
    getRooms(): Promise<void>
    handleTokenRefresh: () => Promise<Response | null>
    getBookings: () => Promise<void>
}

export default function CreateBooking({rooms, setRooms, getRooms, handleTokenRefresh, getBookings}: Props) {
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState<{ text: string; type: "success" | "error" } | null>(null);

    const [formData, setFormData] = useState({
        roomId: "",
        startdate: "",
        starttime: "",
        enddate: "",
        endtime: "",
    });

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setMessage(null);

        try {
            const startDateTime = new Date(`${formData.startdate}T${formData.starttime}`);
            const endDateTime = new Date(`${formData.enddate}T${formData.endtime}`);

            let response = await fetch("http://localhost:5117/room-bookings", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `${localStorage.getItem("accessToken")}`
                },
                body: JSON.stringify({
                    roomId: Number(formData.roomId),
                    startTime: startDateTime.toISOString(),
                    endTime: endDateTime.toISOString(),
                })
            });

            if (response.status === 498) {
                const refreshResult = await handleTokenRefresh();
                if (refreshResult) {
                    response = await fetch("http://localhost:5117/room-bookings", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json",
                            "Authorization": `${localStorage.getItem("accessToken")}`
                        },
                        body: JSON.stringify({
                            roomId: Number(formData.roomId),
                            startTime: startDateTime.toISOString(),
                            endTime: endDateTime.toISOString(),
                        })
                    });
                } else {
                    setMessage({ text: "Authentication failed", type: "error" });
                    setLoading(false);
                    return;
                }
            }

            if (response.ok) {
                setMessage({ text: "Booking created successfully!", type: "success" });
                setFormData({
                    roomId: "",
                    startdate: "",
                    starttime: "",
                    enddate: "",
                    endtime: "",
                });
                await getBookings();
            } else {
                const data = await response.json();
                setMessage({ text: data.message || "Failed to create booking", type: "error" });
            }
        } catch (error) {
            console.error("Error creating booking:", error);
            setMessage({ text: "Error creating booking", type: "error" });
        } finally {
            setLoading(false);
        }
    };

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
    ) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        });
    };

    return (
        <div className={styles.mainContainer}>
            <div className={styles.content}>
                <div className={styles.formContainer}>
                    <h1 className={styles.pageTitle}>Book a room</h1>

                    {message && (
                        <div className={`${styles.message} ${styles[message.type]}`}>
                            {message.text}
                        </div>
                    )}

                    <form onSubmit={handleSubmit} className={styles.form}>
                        <div className={styles.formGroup}>
                            <label htmlFor="roomId" className={styles.label}>
                                Room
                            </label>
                            <select
                                id="roomId"
                                name="roomId"
                                value={formData.roomId}
                                onChange={handleChange}
                                className={styles.select}
                                required
                            >
                                <option value="">Select a room</option>
                                {rooms.map((room) => (
                                    <option key={room.id} value={room.id}>
                                        {room.name}
                                    </option>
                                ))}
                            </select>
                        </div>

                        <div className={styles.dateTimeGroup}>
                            <div className={styles.formGroup}>
                                <label htmlFor="date" className={styles.label}>
                                    Start Date
                                </label>
                                <input
                                    type="date"
                                    id="startdate"
                                    name="startdate"
                                    value={formData.startdate}
                                    onChange={handleChange}
                                    className={styles.input}
                                    required
                                />
                            </div>

                            <div className={styles.formGroup}>
                                <label htmlFor="time" className={styles.label}>
                                    Start Time
                                </label>
                                <input
                                    type="time"
                                    id="starttime"
                                    name="starttime"
                                    value={formData.starttime}
                                    onChange={handleChange}
                                    className={styles.input}
                                    required
                                />
                            </div>

                            <div className={styles.formGroup}>
                                <label htmlFor="date" className={styles.label}>
                                    End Date
                                </label>
                                <input
                                    type="date"
                                    id="enddate"
                                    name="enddate"
                                    value={formData.enddate}
                                    onChange={handleChange}
                                    className={styles.input}
                                    required
                                />
                            </div>

                            <div className={styles.formGroup}>
                                <label htmlFor="time" className={styles.label}>
                                    End Time
                                </label>
                                <input
                                    type="time"
                                    id="endtime"
                                    name="endtime"
                                    value={formData.endtime}
                                    onChange={handleChange}
                                    className={styles.input}
                                    required
                                />
                            </div>
                        </div>

                        <div className={styles.buttonGroup}>
                            <button
                                type="submit"
                                disabled={loading}
                                className={styles.submitButton}
                            >
                                {loading ? "Creating..." : "Create booking"}
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}
