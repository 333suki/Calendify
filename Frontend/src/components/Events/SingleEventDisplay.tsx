import styles from "./EventsDisplay.module.css"

interface Props {
    title: string,
    time: string
}

export default function SingleEventDisplay({title, time}: Props) {
    return (
        <div className={styles.mainContainer}>
            <h1>{title} {time}</h1>

        </div>
    )
}
