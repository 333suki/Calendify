import React from "react";
import styles from "./SelectionMenu.module.css";

interface SelectionMenuProps {
  showEvents: boolean;
  showBirthdays: boolean;
  showAppointments: boolean;
  onToggleEvents: (checked: boolean) => void;
  onToggleBirthdays: (checked: boolean) => void;
  onToggleAppointments: (checked: boolean) => void;
}

export default function SelectionMenu({
  showEvents,
  showBirthdays,
  showAppointments,
  onToggleEvents,
  onToggleBirthdays,
  onToggleAppointments
}: SelectionMenuProps) {
  return (
    <div className={styles.selectionMenu}>
      <h3>Show Events</h3>
      <label>
        <input 
          type="checkbox"
          checked={showEvents}
          onChange={(e) => onToggleEvents(e.target.checked)}
        />
        Events
      </label>
      <label>
        <input 
          type="checkbox"
          checked={showBirthdays}
          onChange={(e) => onToggleBirthdays(e.target.checked)}
        />
        Birthdays
      </label>
      <label>
        <input 
          type="checkbox"
          checked={showAppointments}
          onChange={(e) => onToggleAppointments(e.target.checked)}
        />
        Appointments
      </label>
    </div>
  );
}
