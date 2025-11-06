import React from "react";
import styles from "./Agenda.module.css";

interface Event {
    id: string;
    title: string;
    date: Date;
    time: string;
    type: 'event' | 'birthday' | 'appointment';
}

interface AgendaProps {
    selectedDate: Date;
    events: Event[];
    showBirthdays: boolean;
    showEvents: boolean;
    showAppointments: boolean;
}

export default function Agenda({ 
    selectedDate, 
    events,
    showBirthdays,
    showEvents,
    showAppointments
}: AgendaProps) {
    const weekDayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

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

    const weekDates = getWeekDates(selectedDate);
    const timeSlots = getTimeSlots();

    return (
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
    );
}
