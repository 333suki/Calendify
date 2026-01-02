import React, { useState, useEffect } from "react";
import styles from "./CreateBooking.module.css";
import { useNavigate } from "react-router-dom";

interface Room{
    id: number,
    name: string
}

export default function CreateBooking() {
    const navigate = useNavigate();

    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState<{ text: string; type: "success" | "error" } | null>(null);
    const [rooms, setRooms] = useState<Room[]>([]);

    const [formData, setFormData] = useState({
        roomId: "",
        name: "",
        date: "",
        time: ""    
    });

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

    useEffect(() => {
        const fetchRooms = async () => {
            try {
                let response = await fetch("http://localhost:5117/room-bookings", {
                    headers: {
                        "Authorization": `${localStorage.getItem("accessToken")}`
                    }
                });

                if (response.status === 498) {
                    const refreshResult = await handleTokenRefresh();
                    if (refreshResult) {
                        response = await fetch("http://localhost:5117/room-bookings", {
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

        fetchRooms();
    }, []);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setMessage(null);

        try {
            const dateTime = new Date(`${formData.date}T${formData.time}`);

            let response = await fetch("http://localhost:5117/room-bookings", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `${localStorage.getItem("accessToken")}`
                },
                body: JSON.stringify({
                    roomId: Number(formData.roomId),
                    date: dateTime.toISOString()
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
                            date: dateTime.toISOString()
                        })
                    });
                } else {
                    setMessage({ text: "Authentication failed", type: "error" });
                    setLoading(false);
                    return;
                }
            }

            if (response.ok) {
                setMessage({ text: "booking created successfully!", type: "success" });
                setFormData({
                    roomId: "",
                    name: "",
                    date: "",
                    time: ""
                });
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
                                    Date
                                </label>
                                <input
                                    type="date"
                                    id="date"
                                    name="date"
                                    value={formData.date}
                                    onChange={handleChange}
                                    className={styles.input}
                                    required
                                />
                            </div>

                            <div className={styles.formGroup}>
                                <label htmlFor="time" className={styles.label}>
                                    Time
                                </label>
                                <input
                                    type="time"
                                    id="time"
                                    name="time"
                                    value={formData.time}
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
