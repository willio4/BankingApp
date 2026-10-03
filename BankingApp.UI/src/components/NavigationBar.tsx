import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { library } from "@fortawesome/fontawesome-svg-core";
import { Link } from "react-router-dom";

import { fas } from "@fortawesome/free-solid-svg-icons";
import { far } from "@fortawesome/free-regular-svg-icons";
import { fab } from "@fortawesome/free-brands-svg-icons";
import { useAuth } from "../context/AuthContext";

library.add(fas, far, fab);

export const NavigationBar: React.FC = () => {

  const { user, logout, isLoading } = useAuth();

  if(isLoading) {
    return (
      <h1>Loading...</h1>
    )
  }

  return (
    <div style={styles.navBar}>
      {/* Brand Section */}
      <div style={styles.logo}>
        <FontAwesomeIcon icon={["fas", "building-columns"]} size="lg" />
        <span style={styles.brandName}>ApexBank</span>
      </div>

      {/* Primary Navigation */}
      <nav style={styles.tabs}>
        <ul style={styles.tabsUl}>
          <li style={styles.navItem}>Insurance</li>
          <li style={{ ...styles.navItem, ...styles.activeNavItem }}>
            Banking
          </li>
          <li style={styles.navItem}>Investing</li>
          <li style={styles.navItem}>Life Insurance</li>
          <li style={styles.navItem}>Advice</li>
          <li style={styles.navItem}>Membership</li>
        </ul>
      </nav>

      {/* Right Utilities & Primary Action */}
      <div style={styles.tools}>
        <ul style={styles.toolsUl}>
          <li style={styles.toolItem}>
            <FontAwesomeIcon icon={["fas", "magnifying-glass"]} />
            <span>Search</span>
          </li>
          <li style={styles.toolItem}>
            <FontAwesomeIcon icon={["far", "message"]} />
            <span>Chat 24/7</span>
          </li>
          <li>
            {user ? (
              <div style={styles.userAndLogout}>
                <p style={{ ...styles.greetings,  ...styles.accountButton}}>
                  <FontAwesomeIcon icon={["far", "user-circle"]} />
                  {user?.customer.firstName}
                  <FontAwesomeIcon icon={["fas", "caret-down"]} />
                </p>
                <button onClick={logout} style={styles.logout}>
                  <span>Log Out</span>
                </button>
              </div>
            ) : (
              <Link to="/login" style={styles.logOnButton}>
                <FontAwesomeIcon icon={["far", "user-circle"]} />
                <span>Log On</span>
              </Link>
            )}
          </li>
        </ul>
      </div>
    </div>
  );
};

const styles: Record<string, React.CSSProperties> = {
  navBar: {
    background: "#0d6e3d",
    color: "#ffffff",
    display: "flex",
    alignItems: "center",
    justifyContent: "space-between",
    padding: "40px 40px",
    borderBottom: "1px solid rgba(0, 0, 0, 0.15)",
    boxShadow: "0 2px 4px rgba(0, 0, 0, 0.05)",
    WebkitFontSmoothing: "antialiased",
  },
  logo: {
    display: "flex",
    alignItems: "center",
    gap: "10px",
    cursor: "pointer",
  },
  brandName: {
    fontFamily: "system-ui, -apple-system, sans-serif",
    fontWeight: 700,
    fontSize: "18px",
    letterSpacing: "-0.02em",
  },
  tabs: {
    flex: "1",
    margin: "0 40px",
  },
  tabsUl: {
    fontFamily: "system-ui, -apple-system, sans-serif",
    fontWeight: 500,
    fontSize: "14px",
    listStyleType: "none",
    display: "flex",
    justifyContent: "center",
    gap: "28px",
    margin: 0,
    padding: 0,
  },
  navItem: {
    cursor: "pointer",
    opacity: 0.9,
    transition: "opacity 0.2s ease",
    paddingBottom: "2px",
  },
  activeNavItem: {
    opacity: 1,
    fontWeight: 600,
    borderBottom: "2px solid #ffffff",
  },
  tools: {
    display: "flex",
    alignItems: "center",
  },
  toolsUl: {
    fontFamily: "system-ui, -apple-system, sans-serif",
    fontSize: "14px",
    fontWeight: 500,
    listStyleType: "none",
    display: "flex",
    gap: "20px",
    alignItems: "center",
    margin: 0,
    padding: 0,
  },
  toolItem: {
    display: "flex",
    alignItems: "center",
    gap: "6px",
    cursor: "pointer",
    opacity: 0.9,
  },
  logOnButton: {
    color: "#0d6e3d",
    backgroundColor: "#ffffff",
    padding: "6px 14px",
    borderRadius: "20px",
    fontWeight: 600,
    textDecoration: "none",
    display: "flex",
    alignItems: "center",
    gap: "6px",
    transition: "background-color 0.2s ease",
  },
  accountButton: {
    color: "#0d6e3d",
    backgroundColor: "#ffffff",
    border: 0,
    height: "35px",
    padding: "6px 14px",
    borderRadius: "20px",
    fontWeight: 600,
    textDecoration: "none",
    display: "flex",
    alignItems: "center",
    gap: "6px",
    transition: "background-color 0.2s ease",
    cursor: "pointer",
  },
  greetings: {
    textDecoration: "underline",
    color: "white",
    
  },
  userAndLogout: {
    display: "flex",

  },
  logout: {
    all: "unset",
    cursor: "pointer",
    textDecoration: "underline",
    marginLeft: "10px",
  },
};
