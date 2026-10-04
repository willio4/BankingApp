import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { UserHeader } from "../components/UserHeader";
import { AccountsTab } from "../components/AccountsTab";
import { RecentActivityTab } from "../components/RecentActivityTab";

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
      <UserHeader firstName={firstName} id={id} />

      {/* Main Grid Layout */}
      <div style={styles.pageContent}>
        {/* Left Column: Accounts */}
        <div style={styles.left}>
          <AccountsTab accounts={accounts} />
        </div>

        {/* Right Column: Recent Activity / Quick Tools */}
        <div style={styles.right}>
          <RecentActivityTab />
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
  
};
