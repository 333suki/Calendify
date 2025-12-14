import styles from "./SingleEventDisplay.module.css"

interface Props {
    title: string,
    time: string,
    isSelected: boolean
}

export default function SingleEventDisplay({title, time, isSelected}: Props) {
    return (
        <div className={`${styles.mainContainer} ${isSelected ? styles.selected : ""}`}>
            <h1 className={styles.title}>{title}</h1>
            <h2 className={styles.time}>{time}</h2>
        </div>
    )
}
