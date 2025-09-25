import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import styles from "./Dashboard.module.css";

interface Event {
    id: string;
    title: string;
    date: Date;
    time: string;
    type: 'event' | 'birthday' | 'appointment';
}

export default function Dashboard() {
    const [currentDate, setCurrentDate] = useState(new Date());
    const [selectedDate, setSelectedDate] = useState(new Date());
    const [showBirthdays, setShowBirthdays] = useState(true);
    const [showEvents, setShowEvents] = useState(true);
    const [showAppointments, setShowAppointments] = useState(true);
    const [events, setEvents] = useState<Event[]>([
        {
            id: '1',
            title: 'Team Meeting',
            date: new Date(2025, 8, 25), // September 25, 2025
            time: '09:00',
            type: 'event'
        },
        {
            id: '2',
            title: 'John\'s Birthday',
            date: new Date(2025, 8, 26), // September 26, 2025
            time: '12:00',
            type: 'birthday'
        },
        {
            id: '3',
            title: 'Doctor Appointment',
            date: new Date(2025, 8, 27), // September 27, 2025
            time: '14:30',
            type: 'appointment'
        }
    ]);

    const navigate = useNavigate();

    const monthNames = [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    const dayNames = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
    const weekDayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

    const getFirstDayOfMonth = (date: Date) => {
        return new Date(date.getFullYear(), date.getMonth(), 1);
    };

    const getLastDayOfMonth = (date: Date) => {
        return new Date(date.getFullYear(), date.getMonth() + 1, 0);
    };

    const getDaysInMonth = (date: Date) => {
        const firstDay = getFirstDayOfMonth(date);
        const lastDay = getLastDayOfMonth(date);
        const daysFromPrevMonth = firstDay.getDay();
        const daysInCurrentMonth = lastDay.getDate();
        const totalCells = Math.ceil((daysFromPrevMonth + daysInCurrentMonth) / 7) * 7;
        
        const days = [];
        
        // Previous month days
        const prevMonth = new Date(date.getFullYear(), date.getMonth() - 1, 0);
        for (let i = daysFromPrevMonth - 1; i >= 0; i--) {
            days.push({
                date: new Date(date.getFullYear(), date.getMonth() - 1, prevMonth.getDate() - i),
                isCurrentMonth: false
            });
        }
        
        // Current month days
        for (let day = 1; day <= daysInCurrentMonth; day++) {
            days.push({
                date: new Date(date.getFullYear(), date.getMonth(), day),
                isCurrentMonth: true
            });
        }
        
        // Next month days
        const remainingCells = totalCells - days.length;
        for (let day = 1; day <= remainingCells; day++) {
            days.push({
                date: new Date(date.getFullYear(), date.getMonth() + 1, day),
                isCurrentMonth: false
            });
        }
        
        return days;
    };

    const getWeekDates = (date: Date) => {
        const week = [];
        const startOfWeek = new Date(date);
        const day = startOfWeek.getDay();
        const diff = startOfWeek.getDate() - day;
        startOfWeek.setDate(diff);
        
        for (let i = 0; i < 7; i++) {
            const weekDate = new Date(startOfWeek);
            weekDate.setDate(startOfWeek.getDate() + i);
            week.push(weekDate);
        }
        
        return week;
    };

    const getEventsForDate = (date: Date) => {
        return events.filter(event => {
            if (!showBirthdays && event.type === 'birthday') return false;
            if (!showEvents && event.type === 'event') return false;
            if (!showAppointments && event.type === 'appointment') return false;
            
            return event.date.toDateString() === date.toDateString();
        });
    };

    const getTimeSlots = () => {
        const slots = [];
        for (let hour = 8; hour < 18; hour++) {
            slots.push(`${hour.toString().padStart(2, '0')}:00`);
        }
        return slots;
    };

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

    const handleEventAdd = () => {
        // In a real app, this would open a modal or navigate to an event creation page
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

    const handleLogout = () => {
        localStorage.removeItem("role");
        navigate("/");
    };

    const isDateSelected = (date: Date) => {
        return date.toDateString() === selectedDate.toDateString();
    };

    const weekDates = getWeekDates(selectedDate);
    const timeSlots = getTimeSlots();

    return (
        <div className={styles.mainContainer}>
            <div className={styles.dashboard}>
                <div className={styles.eventButton} onClick={handleEventAdd}>
                </div>
                
                <div className={styles.selectionMenu}>
                    <h3>Show Events</h3>
                    <label>
                        <input 
                            type="checkbox" 
                            checked={showEvents}
                            onChange={(e) => setShowEvents(e.target.checked)}
                        />
                        Events
                    </label>
                    <label>
                        <input 
                            type="checkbox" 
                            checked={showBirthdays}
                            onChange={(e) => setShowBirthdays(e.target.checked)}
                        />
                        Birthdays
                    </label>
                    <label>
                        <input 
                            type="checkbox" 
                            checked={showAppointments}
                            onChange={(e) => setShowAppointments(e.target.checked)}
                        />
                        Appointments
                    </label>
                </div>
                
                <div className={styles.agendaView}>
                    <div className={styles.monthNavigation}>
                        <button onClick={() => navigateMonth('prev')}>← Prev</button>
                        <h3>{monthNames[currentDate.getMonth()]} {currentDate.getFullYear()}</h3>
                        <button onClick={() => navigateMonth('next')}>Next →</button>
                    </div>
                    
                    <div className={styles.calendarHeader}>
                        {dayNames.map(day => (
                            <div key={day} className={styles.dayHeader}>{day}</div>
                        ))}
                    </div>
                    
                    <div className={styles.calendarGrid}>
                        {getDaysInMonth(currentDate).map(({ date, isCurrentMonth }, index) => (
                            <div
                                key={index}
                                className={`${styles.dayCell} ${
                                    !isCurrentMonth ? styles.otherMonth : ''
                                } ${isDateSelected(date) ? styles.selected : ''}`}
                                onClick={() => handleDateSelect(date)}
                            >
                                {date.getDate()}
                            </div>
                        ))}
                    </div>
                </div>
                
                <div className={styles.sideMenu}>
                    <h2>Menu</h2>
                    <div className={`${styles.menuItem} ${styles.profile}`} onClick={() => alert('Profile clicked')}>
                        Profile
                    </div>
                    <div className={`${styles.menuItem} ${styles.settings}`} onClick={() => alert('Settings clicked')}>
                        Settings
                    </div>
                    <div className={`${styles.menuItem} ${styles.logout}`} onClick={handleLogout}>
                        Logout
                    </div>
                </div>
            </div>
            
            <div className={styles.weeklyCalendar}>
                <h2>Week of {selectedDate.toLocaleDateString()}</h2>
                <div className={styles.weekView}>
                    <div className={styles.timeSlot}></div>
                    {weekDates.map((date, index) => (
                        <div key={index} className={styles.weekDay}>
                            <div>{weekDayNames[date.getDay()]}</div>
                            <div>{date.getDate()}</div>
                        </div>
                    ))}
                    
                    {timeSlots.map(time => (
                        <React.Fragment key={time}>
                            <div className={styles.timeSlot}>{time}</div>
                            {weekDates.map((date, dateIndex) => (
                                <div key={`${time}-${dateIndex}`} className={styles.eventSlot}>
                                    {getEventsForDate(date)
                                        .filter(event => event.time.startsWith(time.split(':')[0]))
                                        .map(event => (
                                            <div 
                                                key={event.id} 
                                                className={`${styles.event} ${styles[event.type]}`}
                                                title={`${event.title} at ${event.time}`}
                                            >
                                                {event.title}
                                            </div>
                                        ))
                                    }
                                </div>
                            ))}
                        </React.Fragment>
                    ))}
                </div>
            </div>
        </div>
    );
}