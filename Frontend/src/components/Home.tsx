import styles from "./Home.module.css";

import Navigation from "./Navigation";

export default function Home() {
   
    // const handleEventAdd = () => {
    //     const newEvent: Event = {
    //         id: Date.now().toString(),
    //         title: 'New Event',
    //         date: selectedDate,
    //         time: '10:00',
    //         type: 'event'
    //     };
    //     setEvents(prev => [...prev, newEvent]);
    //     alert('Event added successfully!');
    // };

    return (
        <div className={styles.mainContainer}>
            <div className ={styles.navigationContainer}>
                <Navigation/>
            </div>
            <div className={styles.content}>

            </div>
        </div>
    );
}
