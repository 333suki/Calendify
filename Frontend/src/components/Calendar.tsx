import Navigation from "./Navigation";
import { useState } from "react";

import styles from "./Calendar.module.css";
import GridCalendar from "./GridCalendar";
import Agenda from "./Agenda";

interface Event {
    id: string;
    title: string;
    date: Date;
    time: string;
    type: 'event' | 'birthday' | 'appointment';
}

export default function Calendar() {
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
    
        const navigateMonth = (direction: 'prev' | 'next') => {
            setCurrentDate(prev => {
                const newDate = new Date(prev);
                if (direction === 'prev') {
                    newDate.setMonth(prev.getMonth() - 1);
                } else {
                    newDate.setMonth(prev.getMonth() + 1);
                }
                return newDate;
            });
        };
    
        const handleDateSelect = (date: Date) => {
            setSelectedDate(date);
        };
    
    return (
        <div className={styles.mainContainer}>
            <div className ={styles.navigationContainer}>
                <Navigation/>
            </div>
            <div className={styles.content}>
                <GridCalendar
                    currentDate={currentDate}
                    selectedDate={selectedDate}
                    onDateSelect={handleDateSelect}
                    onNavigateMonth={navigateMonth}
                />
                <Agenda
                    selectedDate={selectedDate}
                    events={events}
                    showBirthdays={showBirthdays}
                    showEvents={showEvents}
                    showAppointments={showAppointments}
                />

            </div>
        </div>
    );
}
