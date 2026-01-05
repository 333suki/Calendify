import styles from "./ProfileHeader.module.css";

interface ProfileHeaderProps {
  profile: {
    username: string;
    email: string;
  };
}

export default function ProfileHeader({ profile }: ProfileHeaderProps) {
  return (
    <div className={styles.header}>
      <div className={styles.avatar}>{profile.username[0].toUpperCase()}</div>
      <div>
        <h1>{profile.username}</h1>
        <p>{profile.email}</p>
      </div>
    </div>
  );
}
