import { useState } from 'react';
import styles from './AdminCreateEventPanel.module.css';

interface Props {
    getEvents: () => Promise<void>
}

export default function AdminCreateEventPanel({getEvents}: Props) {
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState<{ text: string; type: 'success' | 'error' } | null>(null);
    
    const [formData, setFormData] = useState({
        type: 0,
        title: '',
        description: '',
        date: '',
        time: ''
    });

    const handleTokenRefresh = async () => {
        try {
            const response = await fetch("http://localhost:5117/auth/refresh", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `${localStorage.getItem("accessToken")}`,
                },
                body: JSON.stringify({
                    "refreshToken": `${localStorage.getItem("refreshToken")}`
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

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setMessage(null);

        try {
            const dateTime = new Date(`${formData.date}T${formData.time}`);
            let response = await fetch("http://localhost:5117/event", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `${localStorage.getItem("accessToken")}`,
                },
                body: JSON.stringify({
                    type: formData.type,
                    title: formData.title,
                    description: formData.description,
                    date: dateTime.toISOString()
                })
            });

            if (response.status === 498) {
                const refreshResult = await handleTokenRefresh();
                if (refreshResult) {
                    response = await fetch("http://localhost:5117/event", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json",
                            "Authorization": `${localStorage.getItem("accessToken")}`,
                        },
                        body: JSON.stringify({
                            type: formData.type,
                            title: formData.title,
                            description: formData.description,
                            date: dateTime.toISOString()
                        })
                    });
                } else {
                    setMessage({ text: "Authentication failed", type: 'error' });
                    setLoading(false);
                    return;
                }
            }

            if (response.ok) {
                setMessage({ text: "Event created successfully!", type: 'success' });
                // Reset form
                setFormData({
                    type: 0,
                    title: '',
                    description: '',
                    date: '',
                    time: ''
                });
                await getEvents();
            } else if (response.status === 401) {
                setMessage({ text: "You need admin privileges to create events", type: 'error' });
            } else {
                const data = await response.json();
                setMessage({ text: data.message || "Failed to create event", type: 'error' });
            }
        } catch (error) {
            console.error("Error creating event:", error);
            setMessage({ text: "Error creating event", type: 'error' });
        } finally {
            setLoading(false);
        }
    };

    const handleChange = (e: React.ChangeEvent<any>) => {
        const name = e.target.name;
        const value = e.target.value;

        setFormData({
            ...formData,
            [name]: name === "type" ? Number(value) : value
        });
    };

    return (
        <div className={styles.mainContainer}>
            <div className={styles.formContainer}>
                <h1 className={styles.pageTitle}>Create New Event</h1>

                {message && (
                    <div className={`${styles.message} ${styles[message.type]}`}>
                        {message.text}
                    </div>
                )}

                <form onSubmit={handleSubmit} className={styles.form}>
                    <div className={styles.formGroup}>
                        <label htmlFor="type" className={styles.label}>
                            Event Type
                        </label>
                        <select
                            id="type"
                            name="type"
                            value={formData.type}
                            onChange={handleChange}
                            className={styles.select}
                            required
                        >
                            <option value={0}>Event</option>
                            <option value={1}>Meeting</option>
                            <option value={2}>Birthday</option>
                            <option value={3}>Holiday</option>
                            <option value={4}>Training</option>
                            <option value={5}>Social</option>
                            <option value={6}>Incident</option>
                        </select>
                    </div>

                    <div className={styles.formGroup}>
                        <label htmlFor="title" className={styles.label}>
                            Title
                        </label>
                        <input
                            type="text"
                            id="title"
                            name="title"
                            value={formData.title}
                            onChange={handleChange}
                            className={styles.input}
                            placeholder="Enter event title"
                            required
                        />
                    </div>

                    <div className={styles.formGroup}>
                        <label htmlFor="description" className={styles.label}>
                            Description
                        </label>
                        <textarea
                            id="description"
                            name="description"
                            value={formData.description}
                            onChange={handleChange}
                            className={styles.textarea}
                            placeholder="Enter event description"
                            rows={4}
                            required
                        />
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
                            {loading ? "Creating..." : "Create Event"}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}
