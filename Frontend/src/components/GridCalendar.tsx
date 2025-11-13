import styles from "./GridCalendar.module.css";

import arrowIcon from "../assets/arrow.svg";

interface GridCalendarProps {
    currentDate: Date;
    selectedDate: Date;
    onDateSelect: (date: Date) => void;
    onNavigateMonth: (direction: 'prev' | 'next') => void;
}

export default function GridCalendar({ 
    currentDate, 
    selectedDate, 
    onDateSelect, 
    onNavigateMonth 
}: GridCalendarProps) {
    const monthNames = [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    const dayNames = ["S", "M", "T", "W", "T", "F", "S"];

    const getDaysInMonth = (date: Date) => {
        const firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        const lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        const daysFromPrevMonth = firstDay.getDay();
        const daysInCurrentMonth = lastDay.getDate();
        const totalCells = Math.ceil((daysFromPrevMonth + daysInCurrentMonth) / 7) * 7;
        
        const days = [];
        
        const prevMonth = new Date(date.getFullYear(), date.getMonth() - 1, 0);
        for (let i = daysFromPrevMonth - 1; i >= 0; i--) {
            days.push({
                date: new Date(date.getFullYear(), date.getMonth() - 1, prevMonth.getDate() - i),
                isCurrentMonth: false
            });
        }
        
        for (let day = 1; day <= daysInCurrentMonth; day++) {
            days.push({
                date: new Date(date.getFullYear(), date.getMonth(), day),
                isCurrentMonth: true
            });
        }
        
        const remainingCells = totalCells - days.length;
        for (let day = 1; day <= remainingCells; day++) {
            days.push({
                date: new Date(date.getFullYear(), date.getMonth() + 1, day),
                isCurrentMonth: false
            });
        }
        
        return days;
    };

    const isDateSelected = (date: Date) => {
        return date.toDateString() === selectedDate.toDateString();
    };

    return (
        <div className={styles.mainContainer} onWheel={(e) => {
            if (e.deltaY < 0) onNavigateMonth('prev');
            else if (e.deltaY > 0) onNavigateMonth('next');
        }}>
            <div className={styles.monthNavigation}>
                <h3>{monthNames[currentDate.getMonth()]} {currentDate.getFullYear()}</h3>
                <div className={styles.monthNavButtons}>
                    <button className={styles.prevMonthButton} onClick={() => onNavigateMonth('prev')}>
                        <img src={arrowIcon} alt=""/>
                    </button>
                    <button className={styles.nextMonthButton} onClick={() => onNavigateMonth('next')}>
                        <img src={arrowIcon} alt=""/>
                    </button>
                </div>
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
                        onClick={() => onDateSelect(date)}
                    >
                        {date.getDate()}
                    </div>
                ))}
            </div>
        </div>
    );
}
