import {useEffect, useState} from "react";
import styles from "./ProfilePanel.module.css";
import ProfileHeader from "./ProfileHeader";
import ProfileDetails from "./ProfileDetails";

// Temp data
export default function ProfilePanel() {
  const [profile, setProfile] = useState({
    username: "johndoe",
    email: "johndoe@example.com",
    newPassword: "",
    confirmPassword: "",
  });

  const [originalProfile, setOriginalProfile] = useState(profile);
  const [isEditing, setIsEditing] = useState(false);
  const [statusMessage, setStatusMessage] = useState<string | null>(null);
  const [statusType, setStatusType] = useState<"success" | "error" | null>(null);

  useEffect(() => {
    getOwnProfileInfo();
  })

  const getOwnProfileInfo = async () => {
    try {
      let response = await fetch("http://localhost:5117/profile", {
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
          response = await fetch("http://localhost:5117/profile", {
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

          setProfile({
            ...profile,
            username: data.username,
            email: data.email
          })

        } catch (err) {
          console.error("Failed to parse JSON:", err);
        }
      }
    } catch (e) {
      console.error(e);
    }
  };

  // Update local state when input changes
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setProfile((prev) => ({ ...prev, [name]: value }));

    // Clear status message when user edits
    if (statusMessage) {
      setStatusMessage(null);
      setStatusType(null);
    }
  };

  // Save changes (username/email/password)
  const handleSave = async () => {
    // Frontend validation msg for password match
    if (profile.newPassword && profile.newPassword !== profile.confirmPassword) {
      setStatusMessage("Passwords do not match.");
      setStatusType("error");
      return;
    }

    // Detect if any changes were made
    if (
      profile.username === originalProfile.username &&
      profile.email === originalProfile.email &&
      !profile.newPassword
    ) {
      setStatusMessage("No changes detected.");
      setStatusType("error");
      return;
    }

    // create a payload to send to the backend
    const payload: any = {
      Username: profile.username,
      Email: profile.email,
    };

    if (profile.newPassword) {
      payload.Password = profile.newPassword;
    }

    try {
      const res = await fetch("http://localhost:5117/profile", {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      });

      const data = await res.json();

      if (!res.ok) {
        setStatusMessage(data.message || "Failed to update profile.");
        setStatusType("error");
        return;
      }

      // On success reset the state
      setOriginalProfile({ ...profile, newPassword: "", confirmPassword: "" });
      setProfile((prev) => ({ ...prev, newPassword: "", confirmPassword: "" }));
      setIsEditing(false);
      setStatusMessage("Profile updated successfully!");
      setStatusType("success");
    } catch (err) {
      setStatusMessage("Server error. Please try again.");
      setStatusType("error");
    }
  };

  return (
    <div className={styles.container}>
      <ProfileHeader profile={profile} />

      <ProfileDetails
        profile={profile}
        isEditing={isEditing}
        onChange={handleChange}
        includePassword={true}
      />

      {statusMessage && (
        <p
          style={{
            color: statusType === "error" ? "#ff6b6b" : "#4ade80",
            marginTop: "0.5rem",
          }}
        >
          {statusMessage}
        </p>
      )}

      <div className={styles.actions}>
        {isEditing ? (
          <>
            <button onClick={() => setIsEditing(false)}>Cancel</button>
            <button onClick={handleSave}>Save Changes</button>
          </>
        ) : (
          <button onClick={() => setIsEditing(true)}>Edit Profile</button>
        )}
      </div>
    </div>
  );
}
