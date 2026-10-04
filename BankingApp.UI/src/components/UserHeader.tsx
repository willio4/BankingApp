import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faInbox,
  faPhone,
  faFileLines,
} from "@fortawesome/free-solid-svg-icons";

export function UserHeader({
  firstName,
  id,
}: {
  firstName: string;
  id: string;
}) {
  {
    /* Header Banner */
  }
  return (
    <div style={styles.heading}>
      <div style={styles.greetingGroup}>
        <h2 style={styles.greeting}>Good Evening, {firstName}</h2>
        <span style={styles.memberId}>Member: *{id?.slice(-6)}</span>
      </div>
      <ul style={styles.shortcuts}>
        <li style={styles.shortcutItem}>
          <FontAwesomeIcon icon={faInbox} />
          <span>Inbox</span>
        </li>
        <li style={styles.shortcutItem}>
          <FontAwesomeIcon icon={faFileLines} />
          <span>Documents</span>
        </li>
        <li style={styles.shortcutItem}>
          <FontAwesomeIcon icon={faPhone} />
          <span>Help</span>
        </li>
      </ul>
    </div>
  );
}

const styles: Record<string, React.CSSProperties> = {
  heading: {
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    background: "#0d6e3d",
    color: "#ffffff",
    padding: "20px 32px",
    borderRadius: "12px",
    marginBottom: "24px",
  },
  greetingGroup: {
    display: "flex",
    flexDirection: "column",
    gap: "4px",
  },
  greeting: {
    margin: 0,
    fontSize: "26px",
    fontWeight: 700,
    color: "inherit",
  },
  memberId: {
    color: "#a7f3d0",
    fontSize: "14px",
    textAlign: "left",
    paddingLeft: "12px",
  },
  shortcuts: {
    display: "flex",
    listStyleType: "none",
    gap: "20px",
    margin: 0,
    padding: 0,
  },
  shortcutItem: {
    display: "flex",
    alignItems: "center",
    gap: "8px",
    cursor: "pointer",
    fontSize: "14px",
    fontWeight: 500,
    opacity: 0.9,
  },
};
