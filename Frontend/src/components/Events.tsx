import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import styles from "./Events.module.css";
import SelectionMenu from "./SelectionMenu";
import Navigation from "./Navigation";

interface Event {
    id: string;
    title: string;
    date: Date;
    time: string;
    type: 'event' | 'birthday' | 'appointment';
}

export default function Events() {
    
    const navigate = useNavigate();

    const handleLogout = () => {
        navigate("/");
    };


    const [currentDate, setCurrentDate] = useState(new Date());
    const [selectedDate, setSelectedDate] = useState(new Date());
    const [showBirthdays, setShowBirthdays] = useState(true);
    const [showEvents, setShowEvents] = useState(true);
    const [showAppointments, setShowAppointments] = useState(true);
    const [events, setEvents] = useState<Event[]>([
        {
            id: '1',
            title: 'Team Meeting',
            date: new Date(2025, 8, 25),
            time: '09:00',
            type: 'event'
        },
        {
            id: '2',
            title: 'John\'s Birthday',
            date: new Date(2025, 8, 26),
            time: '12:00',
            type: 'birthday'
        },
        {
            id: '3',
            title: 'Doctor Appointment',
            date: new Date(2025, 8, 27),
            time: '14:30',
            type: 'appointment'
        }
    ]);

    const handleEventAdd = () => {
        const newEvent: Event = {
            id: Date.now().toString(),
            title: 'New Event',
            date: selectedDate,
            time: '10:00',
            type: 'event'
        };
        setEvents(prev => [...prev, newEvent]);
        alert('Event added successfully!');
    };

    const handleDateSelect = (date: Date) => {
        setSelectedDate(date);
    };

    const navigateMonth = (direction: 'prev' | 'next') => {
        setCurrentDate(prev => {
            const newDate = new Date(prev);
            if (direction === 'prev') newDate.setMonth(prev.getMonth() - 1);
            else newDate.setMonth(prev.getMonth() + 1);
            return newDate;
        });
    };

    return (
        <div className={styles.mainContainer}>
            <Navigation onLogout={handleLogout} />


            <div className={styles.HomeContainer}>
                <button className={styles.eventButton} onClick={handleEventAdd}>
                    Add Event
                </button>
            </div>

            <div className={styles.eventsList}>
                {events.map(event => (
                    <div key={event.id} className={styles.eventItem}>
                        <strong>{event.title}</strong> - {event.date.toDateString()} {event.time} ({event.type})
                    </div>
                ))}
            </div>

            <SelectionMenu
            showEvents={showEvents}
            showBirthdays={showBirthdays}
            showAppointments={showAppointments}
            onToggleEvents={(checked) => setShowEvents(checked)}
            onToggleBirthdays={(checked) => setShowBirthdays(checked)}
            onToggleAppointments={(checked) => setShowAppointments(checked)}
            />
        </div>
    );
}
