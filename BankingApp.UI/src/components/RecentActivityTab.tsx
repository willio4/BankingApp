
export function RecentActivityTab() {


    return (
      <div style={styles.quickToolsCard}>
        <h3 style={styles.sectionTitle}>Recent Activity</h3>
        <p style={styles.placeholderText}>
          Select an account to view recent transactions and details.
        </p>
      </div>
    );
}

const styles: Record<string, React.CSSProperties> = {
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