import React from "react";

import styles from "./Dashboard.module.css"

const Dashboard: React.FC = () => {
    return (
        <div>
            <h1 className={styles.welcomeText}>Welcome to the Admin Dashboard</h1>
        </div>
    );
};

export default Dashboard;
