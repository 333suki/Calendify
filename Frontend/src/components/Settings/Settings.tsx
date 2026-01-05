import styles from "./Settings.module.css";
import Navigation from "../Navigation";
import { useTheme, type Theme } from "./ThemeContext";

export default function Settings() {
    const { theme, setTheme } = useTheme();

    const themes: { value: Theme; label: string; preview: string }[] = [
        { value: 'light', label: 'Light', preview: '#f5f7fa' },
        { value: 'dark', label: 'Dark', preview: '#1a202c' },
        { value: 'blue', label: 'Ocean Blue', preview: '#0f2027' },
        { value: 'purple', label: 'Purple Dream', preview: '#2d1b69' },
        { value: 'green', label: 'Forest Green', preview: '#134e4a' },
    ];

    return (
        <div className={styles.mainContainer}>
            <div className={styles.navigationContainer}>
                <Navigation/>
            </div>
            <div className={styles.content}>
                <div className={styles.settingsContainer}>
                    <h1 className={styles.pageTitle}>Settings</h1>

                    {/* Theme Section */}
                    <div className={styles.section}>
                        <h2 className={styles.sectionTitle}>Color Theme</h2>
                        
                        <div className={styles.themeGrid}>
                            {themes.map((themeOption) => (
                                <button
                                    key={themeOption.value}
                                    onClick={() => setTheme(themeOption.value)}
                                    className={`${styles.themeCard} ${
                                        theme === themeOption.value ? styles.themeCardActive : ''
                                    }`}
                                >
                                    <div className={styles.colorPreview}>
                                        <div 
                                            className={styles.colorCircle}
                                            style={{ background: themeOption.preview }}
                                        ></div>
                                        <div 
                                            className={styles.colorCircle}
                                            style={{ background: themeOption.preview, opacity: 0.7 }}
                                        ></div>
                                        <div 
                                            className={styles.colorCircle}
                                            style={{ background: themeOption.preview, opacity: 0.4 }}
                                        ></div>
                                    </div>
                                    
                                    <span className={styles.themeName}>{themeOption.label}</span>
                                    
                                    {theme === themeOption.value && (
                                        <div className={styles.checkmark}>✓</div>
                                    )}
                                </button>
                            ))}
                        </div>

                        <div className={styles.currentTheme}>
                            Current theme: <span className={styles.themeBold}>
                                {themes.find(t => t.value === theme)?.label}
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}