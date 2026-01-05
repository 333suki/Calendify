import {useEffect, useState} from "react";
import flameIcon from "../../assets/flame.svg";
import styles from "./StreakDisplay.module.css"

export default function StreakDisplay() {
    const [count, setCount] = useState(0);

    useEffect(() => {
        getOwnStreak();
    })

    const getOwnStreak = async () => {
        try {
            let response = await fetch("http://localhost:5117/streak", {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `${localStorage.getItem("accessToken")}`,
                }
            });
            if (response.status === 498) {
                console.log("Got 498");
                console.log("Doing refresh request");
                response = await fetch("http://localhost:5117/auth/refresh", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `${localStorage.getItem("accessToken")}`,
                    },
                    body: JSON.stringify({
                        "refreshToken": `${localStorage.getItem("refreshToken")}`
                    })
                });
                let data = null;
                const text = await response.text();
                if (text) {
                    try {
                        data = JSON.parse(text);
                    } catch (err) {
                        console.error("Failed to parse JSON:", err);
                    }
                }
                if (response.ok) {
                    console.log("Refresh request OK");
                    if (data) {
                        localStorage.setItem("accessToken", data.accessToken);
                        localStorage.setItem("refreshToken", data.refreshToken);
                    }
                    console.log("Updated accessToken and refreshToken");

                    // Try again
                    response = await fetch("http://localhost:5117/streak", {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json",
                            "Authorization": `${localStorage.getItem("accessToken")}`,
                        }
                    });
                }
            }
            let data = null;
            const text = await response.text();
            if (text) {
                try {
                    data = JSON.parse(text);
                    setCount(data.count);
                } catch (err) {
                    console.error("Failed to parse JSON:", err);
                }
            }
        } catch (e) {
            console.error(e);
        }
    };

    return (
        <div className={styles.mainContainer}>
            <div className={styles.countDisplay}>
                {count}
            </div>
            <div className={styles.flameIcon}>
                <img src={flameIcon} alt="Person"></img>
            </div>
        </div>
    )
}
