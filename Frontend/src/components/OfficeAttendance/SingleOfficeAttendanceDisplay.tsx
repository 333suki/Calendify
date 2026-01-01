import styles from "./SingleOfficeAttendanceDisplay.module.css"
import {useEffect, useState} from "react";

interface Props {
    date: Date,
    isSelected: boolean
}

function formatDateYYYYMMDD(date: Date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");

    return `${year}-${month}-${day}`;
}

interface OfficeAttendance {
    status: number
}

const STATUS = {
    ATTENDING: 0,
    ABSENT: 1,
};

export default function SingleOfficeAttendanceDisplay({date, isSelected}: Props) {
    const [attendance, setAttendance] = useState<OfficeAttendance | null>(null);

    const updateAttendance = async (newStatus: number) => {
        try {
            let response = await fetch("http://localhost:5117/officeattendance", {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `${localStorage.getItem("accessToken")}`,
                },
                body: JSON.stringify({
                    date: formatDateYYYYMMDD(date),
                    attendanceStatus: newStatus,
                }),
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
                    response = await fetch("http://localhost:5117/officeattendance", {
                        method: "PUT",
                        headers: {
                            "Content-Type": "application/json",
                            "Authorization": `${localStorage.getItem("accessToken")}`,
                        },
                        body: JSON.stringify({
                            date: formatDateYYYYMMDD(date),
                            attendanceStatus: newStatus,
                        }),
                    });
                }
            }
        } catch (e) {
            console.error(e);
        }
    };

    useEffect(() => {
        const getAttendance = async () => {
            try {
                let response = await fetch(`http://localhost:5117/officeattendance?date=${formatDateYYYYMMDD(date)}`, {
                    method: "GET",
                    headers: {
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
                        response = await fetch(`http://localhost:5117/officeattendance?date=${formatDateYYYYMMDD(date)}`, {
                            method: "GET",
                            headers: {
                                "Authorization": `${localStorage.getItem("accessToken")}`,
                            }
                        });
                    }
                }

                setAttendance(await response.json());
            } catch (e) {
                console.error(e);
            }
        };

        getAttendance();
    }, []);

    return (
        <select
            className={`${styles.attendanceSelect} ${isSelected ? styles.selected : ""} ${attendance?.status == null || attendance?.status == STATUS.ATTENDING ? styles.attending : styles.absent}`}
            value={attendance?.status ?? ""}
            onChange={async (e) => {
                if (!attendance) return;

                const newStatus = Number(e.target.value);

                setAttendance({
                    ...attendance,
                    status: newStatus,
                });

                try {
                    await updateAttendance(newStatus);
                } catch (err) {
                    console.error(err);
                    setAttendance(attendance);
                }
            }}>
            <option value={STATUS.ATTENDING}>Attending</option>
            <option value={STATUS.ABSENT}>Absent</option>
        </select>
    )
}
