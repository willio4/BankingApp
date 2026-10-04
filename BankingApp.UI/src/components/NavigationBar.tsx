import React from "react";
import { Link } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faBuildingColumns,
  faMagnifyingGlass,
  faCaretDown,
} from "@fortawesome/free-solid-svg-icons";
import { faMessage, faUserCircle } from "@fortawesome/free-regular-svg-icons";
import { useAuth } from "../context/AuthContext";

export const NavigationBar: React.FC = () => {
  const { user, logout, isLoading } = useAuth();

  if (isLoading) {
    return (
      <div style={styles.loadingContainer}>
        <span>Loading...</span>
      </div>
    );
  }

  return (
    <div style={styles.navBar}>
      {/* Brand Section */}
      <div style={styles.logo}>
        <FontAwesomeIcon icon={faBuildingColumns} size="lg" />
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
            <FontAwesomeIcon icon={faMagnifyingGlass} />
            <span>Search</span>
          </li>
          <li style={styles.toolItem}>
            <FontAwesomeIcon icon={faMessage} />
            <span>Chat 24/7</span>
          </li>
          <li>
            {user ? (
              <div style={styles.userAndLogout}>
                <button style={styles.accountButton}>
                  <FontAwesomeIcon icon={faUserCircle} />
                  <span>{user?.customer?.firstName}</span>
                  <FontAwesomeIcon icon={faCaretDown} />
                </button>
                <button onClick={logout} style={styles.logout}>
                  Log Out
                </button>
              </div>
            ) : (
              <Link to="/login" style={styles.logOnButton}>
                <FontAwesomeIcon icon={faUserCircle} />
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
  loadingContainer: {
    background: "#0d6e3d",
    color: "#ffffff",
    padding: "16px 32px",
    fontFamily: "system-ui, -apple-system, sans-serif",
  },
  navBar: {
    background: "#0d6e3d",
    color: "#ffffff",
    display: "flex",
    alignItems: "center",
    justifyContent: "space-between",
    padding: "16px 32px",
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
    fontSize: "15px",
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
    padding: "8px 16px",
    borderRadius: "20px",
    fontWeight: 600,
    textDecoration: "none",
    display: "flex",
    alignItems: "center",
    gap: "6px",
    transition: "background-color 0.2s ease",
  },
  userAndLogout: {
    display: "flex",
    alignItems: "center",
    gap: "12px",
  },
  accountButton: {
    color: "#0d6e3d",
    backgroundColor: "#ffffff",
    border: "none",
    padding: "8px 16px",
    borderRadius: "20px",
    fontWeight: 600,
    display: "flex",
    alignItems: "center",
    gap: "6px",
    cursor: "pointer",
    fontFamily: "inherit",
    fontSize: "14px",
  },
  logout: {
    background: "none",
    border: "none",
    color: "#ffffff",
    cursor: "pointer",
    textDecoration: "underline",
    fontSize: "14px",
    padding: 0,
    fontFamily: "inherit",
  },
};
