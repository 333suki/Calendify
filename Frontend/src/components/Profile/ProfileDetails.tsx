import styles from "./ProfileDetails.module.css";

interface ProfileDetailsProps {
  profile: any;
  isEditing: boolean;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  includePassword?: boolean;
}

export default function ProfileDetails({
  profile,
  isEditing,
  onChange,
  includePassword,
}: ProfileDetailsProps) {
  return (
    <div className={styles.container}>
      <div className={styles.field}>
        <label>Username</label>
        {isEditing ? (
          <input name="username" value={profile.username} onChange={onChange} />
        ) : (
          <p>{profile.username}</p>
        )}
      </div>

      <div className={styles.field}>
        <label>Email</label>
        {isEditing ? (
          <input
            type="email"
            name="email"
            value={profile.email}
            onChange={onChange}
          />
        ) : (
          <p>{profile.email}</p>
        )}
      </div>

      {includePassword && isEditing && (
        <>
          <div className={styles.field}>
            <label>New Password</label>
            <input
              type="password"
              name="newPassword"
              value={profile.newPassword}
              onChange={onChange}
            />
          </div>

          <div className={styles.field}>
            <label>Confirm New Password</label>
            <input
              type="password"
              name="confirmPassword"
              value={profile.confirmPassword}
              onChange={onChange}
            />
          </div>
        </>
      )}
    </div>
  );
}
