import styles from "./RoomBookingPanel.module.css";
import RoomDisplay from "./BookingDisplay";
import Booking from "./CreateBooking";


type Props = {
    selectedDate: Date;
};

export default function RoomBookingPanel() {
    return (
        <div className={styles.mainContainer}>
            <Booking />
            <RoomDisplay />
        </div>
    );
}
