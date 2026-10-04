import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faArrowRightArrowLeft,
  faCreditCard,
  faInbox,
  faPhone,
  faPlus,
  faFileLines,
} from "@fortawesome/free-solid-svg-icons";

export function DashboardPage() {
  const { user, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (!isAuthenticated) {
      navigate("/login");
    }
  }, [isAuthenticated, navigate]);

  if (!user || !user.customer) {
    return null;
  }

  const { firstName, id, accounts = [] } = user.customer;

  return (
    <div style={styles.container}>
      {/* Header Banner */}
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

      {/* Main Grid Layout */}
      <div style={styles.pageContent}>
        {/* Left Column: Accounts */}
        <div style={styles.left}>
          <div style={styles.accountsCard}>
            <div style={styles.accountsHeader}>
              <span style={styles.accountsTitle}>Accounts</span>
              <div style={styles.accountsActions}>
                <button style={styles.actionButton}>
                  <FontAwesomeIcon icon={faArrowRightArrowLeft} />
                  <span>Transfer</span>
                </button>
                <button style={styles.actionButton}>
                  <FontAwesomeIcon icon={faCreditCard} />
                  <span>Pay bills</span>
                </button>
                <button style={styles.actionButton}>
                  <FontAwesomeIcon icon={faFileLines} />
                  <span>Statements</span>
                </button>
                <button style={styles.actionButton}>
                  <FontAwesomeIcon icon={faPlus} />
                  <span>Add account</span>
                </button>
              </div>
            </div>

            <div style={styles.accountsList}>
              {accounts.map((account) => (
                <div key={account.id} style={styles.accountEntry}>
                  <div style={styles.accountInfo}>
                    <span style={styles.accountName}>
                      {account.type === "Checking"
                        ? "ApexBank Checking"
                        : "ApexBank Savings"}
                    </span>
                    <span style={styles.accountNumber}>
                      *{account.accountNumber?.slice(-4)}
                    </span>
                  </div>

                  <div style={styles.accountMeta}>
                    <span style={styles.accountBalance}>
                      ${account.balance?.toFixed(2)}
                    </span>
                    <button style={styles.linkButton}>Deposit</button>
                    <button style={styles.linkButton}>Withdraw</button>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>

        {/* Right Column: Recent Activity / Quick Tools */}
        <div style={styles.right}>
          <div style={styles.quickToolsCard}>
            <h3 style={styles.sectionTitle}>Recent Activity</h3>
            <p style={styles.placeholderText}>
              Select an account to view recent transactions and details.
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}

export default DashboardPage;

const styles: Record<string, React.CSSProperties> = {
  container: {
    padding: "24px 32px",
    fontFamily: "system-ui, -apple-system, sans-serif",
    backgroundColor: "#f8fafc",
    minHeight: "100vh",
    boxSizing: "border-box",
  },
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
    color: "inherit"
  },
  memberId: {
    color: "#a7f3d0",
    fontSize: "14px",
    textAlign: "left",
    paddingLeft: "12px"
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
  pageContent: {
    display: "grid",
    gridTemplateColumns: "1fr 1fr",
    gap: "24px",
  },
  left: {
    display: "flex",
    flexDirection: "column",
  },
  right: {
    display: "flex",
    flexDirection: "column",
  },
  accountsCard: {
    backgroundColor: "#ffffff",
    borderRadius: "12px",
    border: "1px solid #e2e8f0",
    overflow: "hidden",
    boxShadow: "0 1px 3px rgba(0,0,0,0.05)",
  },
  accountsHeader: {
    backgroundColor: "#12395b",
    color: "#ffffff",
    padding: "14px 20px",
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
  },
  accountsTitle: {
    fontWeight: "700",
    fontSize: "18px",
  },
  accountsActions: {
    display: "flex",
    gap: "16px",
  },
  actionButton: {
    background: "none",
    border: "none",
    color: "#ffffff",
    display: "flex",
    alignItems: "center",
    gap: "6px",
    cursor: "pointer",
    fontSize: "13px",
    padding: 0,
    opacity: 0.9,
  },
  accountsList: {
    padding: "16px",
    display: "flex",
    flexDirection: "column",
    gap: "12px",
  },
  accountEntry: {
    border: "1px solid #cbd5e1",
    borderRadius: "10px",
    padding: "16px",
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    backgroundColor: "#ffffff",
  },
  accountInfo: {
    display: "flex",
    alignItems: "center",
    gap: "10px",
  },
  accountName: {
    fontWeight: "700",
    fontSize: "16px",
    color: "#0f172a",
  },
  accountNumber: {
    fontSize: "13px",
    color: "#64748b",
  },
  accountMeta: {
    display: "flex",
    alignItems: "center",
    gap: "16px",
  },
  accountBalance: {
    fontWeight: "700",
    fontSize: "16px",
    color: "#0f172a",
  },
  linkButton: {
    background: "none",
    border: "none",
    color: "#0d6e3d",
    fontWeight: "600",
    fontSize: "14px",
    cursor: "pointer",
    padding: 0,
  },
  quickToolsCard: {
    backgroundColor: "#ffffff",
    borderRadius: "12px",
    border: "1px solid #e2e8f0",
    padding: "20px",
    boxShadow: "0 1px 3px rgba(0,0,0,0.05)",
  },
  sectionTitle: {
    margin: "0 0 12px 0",
    fontSize: "18px",
    color: "#0f172a",
  },
  placeholderText: {
    color: "#64748b",
    fontSize: "14px",
    margin: 0,
  },
};
