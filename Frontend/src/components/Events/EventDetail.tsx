import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import styles from './EventDetail.module.css';
import Navigation from '../Navigation';

interface Event {
    id: number;
    title: string;
    description: string;
    date: string;
    type: number;
}

export default function EventDetail() {
    const { eventId } = useParams<{ eventId: string }>();
    const navigate = useNavigate();
    const [event, setEvent] = useState<Event | null>(null);
    const [isRegistered, setIsRegistered] = useState(false);
    const [loading, setLoading] = useState(true);
    const [actionLoading, setActionLoading] = useState(false);
    const [message, setMessage] = useState<{ text: string; type: 'success' | 'error' } | null>(null);

    useEffect(() => {
        fetchEventDetails();
        checkRegistrationStatus();
    }, [eventId]);

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

    const fetchEventDetails = async () => {
        try {
            const today = new Date();
            const events: Event[] = [];
            
            for (let i = 0; i < 60; i++) {
                const date = new Date(today);
                date.setDate(today.getDate() + i);
                const dateString = formatDateYYYYMMDD(date);
                
                let response = await fetch(`http://localhost:5117/event?date=${dateString}`, {
                    method: "GET",
                    headers: {
                        "Authorization": `${localStorage.getItem("accessToken")}`,
                    }
                });

                if (response.status === 498) {
                    const refreshResult = await handleTokenRefresh();
                    if (refreshResult) {
                        response = await fetch(`http://localhost:5117/event?date=${dateString}`, {
                            method: "GET",
                            headers: {
                                "Authorization": `${localStorage.getItem("accessToken")}`,
                            }
                        });
                    } else {
                        continue;
                    }
                }

                if (response.ok) {
                    const dayEvents = await response.json();
                    events.push(...dayEvents);
                    
                    // Check if we found our event
                    const foundEvent = dayEvents.find((e: Event) => e.id === parseInt(eventId || '0'));
                    if (foundEvent) {
                        setEvent(foundEvent);
                        break;
                    }
                }
            }
        } catch (error) {
            console.error("Error fetching event details:", error);
        } finally {
            setLoading(false);
        }
    };

    const checkRegistrationStatus = async () => {
        try {
            let response = await fetch("http://localhost:5117/eventattendance", {
                method: "GET",
                headers: {
                    "Authorization": `${localStorage.getItem("accessToken")}`,
                }
            });

            if (response.status === 498) {
                const refreshResult = await handleTokenRefresh();
                if (refreshResult) {
                    response = await fetch("http://localhost:5117/eventattendance", {
                        method: "GET",
                        headers: {
                            "Authorization": `${localStorage.getItem("accessToken")}`,
                        }
                    });
                } else {
                    return;
                }
            }

            if (response.ok) {
                const attendances = await response.json();
                const isAttending = attendances.some((a: any) => a.eventID === parseInt(eventId || '0'));
                setIsRegistered(isAttending);
            }
        } catch (error) {
            console.error("Error checking registration:", error);
        }
    };

    const handleRegisterAttendance = async () => {
        setActionLoading(true);
        setMessage(null);
        
        try {
            let response = await fetch("http://localhost:5117/eventattendance", {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `${localStorage.getItem("accessToken")}`,
                },
                body: JSON.stringify({
                    eventID: parseInt(eventId || '0')
                })
            });

            if (response.status === 498) {
                const refreshResult = await handleTokenRefresh();
                if (refreshResult) {
                    response = await fetch("http://localhost:5117/eventattendance", {
                        method: "PUT",
                        headers: {
                            "Content-Type": "application/json",
                            "Authorization": `${localStorage.getItem("accessToken")}`,
                        },
                        body: JSON.stringify({
                            eventID: parseInt(eventId || '0')
                        })
                    });
                } else {
                    setMessage({ text: "Authentication failed", type: 'error' });
                    setActionLoading(false);
                    return;
                }
            }

            if (response.ok) {
                setIsRegistered(true);
                setMessage({ text: "Successfully registered for event!", type: 'success' });
            } else {
                const data = await response.json();
                setMessage({ text: data.message || "Failed to register", type: 'error' });
            }
        } catch (error) {
            setMessage({ text: "Error registering for event", type: 'error' });
        } finally {
            setActionLoading(false);
        }
    };

    const handleUnregisterAttendance = async () => {
        setActionLoading(true);
        setMessage(null);
        
        try {
            let response = await fetch(`http://localhost:5117/eventattendance/${eventId}`, {
                method: "DELETE",
                headers: {
                    "Authorization": `${localStorage.getItem("accessToken")}`,
                }
            });

            if (response.status === 498) {
                const refreshResult = await handleTokenRefresh();
                if (refreshResult) {
                    response = await fetch(`http://localhost:5117/eventattendance/${eventId}`, {
                        method: "DELETE",
                        headers: {
                            "Authorization": `${localStorage.getItem("accessToken")}`,
                        }
                    });
                } else {
                    setMessage({ text: "Authentication failed", type: 'error' });
                    setActionLoading(false);
                    return;
                }
            }

            if (response.ok) {
                setIsRegistered(false);
                setMessage({ text: "Successfully unregistered from event", type: 'success' });
            } else {
                const data = await response.json();
                setMessage({ text: data.message || "Failed to unregister", type: 'error' });
            }
        } catch (error) {
            setMessage({ text: "Error unregistering from event", type: 'error' });
        } finally {
            setActionLoading(false);
        }
    };

    const formatDateYYYYMMDD = (date: Date) => {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, "0");
        const day = String(date.getDate()).padStart(2, "0");
        return `${year}-${month}-${day}`;
    };

    const formatEventDate = (dateString: string) => {
        const date = new Date(dateString);
        return date.toLocaleDateString("en-US", {
            weekday: "long",
            year: "numeric",
            month: "long",
            day: "numeric"
        });
    };

    const formatEventTime = (dateString: string) => {
        const date = new Date(dateString);
        return date.toLocaleTimeString("en-GB", {
            hour: "2-digit",
            minute: "2-digit",
            hour12: false
        });
    };

    if (loading) {
        return (
            <div className={styles.mainContainer}>
                <div className={styles.navigationContainer}>
                    <Navigation />
                </div>
                <div className={styles.content}>
                    <div className={styles.detailContainer}>
                        <div className={styles.loadingMessage}>Loading event details...</div>
                    </div>
                </div>
            </div>
        );
    }

    if (!event) {
        return (
            <div className={styles.mainContainer}>
                <div className={styles.navigationContainer}>
                    <Navigation />
                </div>
                <div className={styles.content}>
                    <div className={styles.detailContainer}>
                        <div className={styles.errorMessage}>Event not found</div>
                        <button onClick={() => navigate('/events')} className={styles.backButton}>
                            Back to Events
                        </button>
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className={styles.mainContainer}>
            <div className={styles.navigationContainer}>
                <Navigation />
            </div>
            <div className={styles.content}>
                <div className={styles.detailContainer}>
                    <button onClick={() => navigate('/events')} className={styles.backButton}>
                        ← Back to Events
                    </button>

                    <div className={styles.eventCard}>
                        <div className={styles.eventHeader}>
                            <h1 className={styles.eventTitle}>{event.title}</h1>
                            <span className={styles.eventType}>
                                {event.type === 0 ? "Event" : "Meeting"}
                            </span>
                        </div>

                        <div className={styles.eventDateTime}>
                            <div className={styles.dateTimeItem}>
                                <span className={styles.icon}>📅</span>
                                <span>{formatEventDate(event.date)}</span>
                            </div>
                            <div className={styles.dateTimeItem}>
                                <span className={styles.icon}>🕐</span>
                                <span>{formatEventTime(event.date)}</span>
                            </div>
                        </div>

                        <div className={styles.eventDescription}>
                            <h2 className={styles.sectionTitle}>Description</h2>
                            <p>{event.description}</p>
                        </div>

                        <div className={styles.attendanceSection}>
                            <h2 className={styles.sectionTitle}>Attendance</h2>
                            
                            {message && (
                                <div className={`${styles.message} ${styles[message.type]}`}>
                                    {message.text}
                                </div>
                            )}

                            <div className={styles.attendanceStatus}>
                                <span>Status: </span>
                                <span className={isRegistered ? styles.registered : styles.notRegistered}>
                                    {isRegistered ? "✓ Registered" : "Not Registered"}
                                </span>
                            </div>

                            {isRegistered ? (
                                <button
                                    onClick={handleUnregisterAttendance}
                                    disabled={actionLoading}
                                    className={`${styles.actionButton} ${styles.unregisterButton}`}
                                >
                                    {actionLoading ? "Processing..." : "Unregister from Event"}
                                </button>
                            ) : (
                                <button
                                    onClick={handleRegisterAttendance}
                                    disabled={actionLoading}
                                    className={`${styles.actionButton} ${styles.registerButton}`}
                                >
                                    {actionLoading ? "Processing..." : "Register for Event"}
                                </button>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}