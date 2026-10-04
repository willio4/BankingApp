import type { Account } from "../types/api";

export function Account({ account }: { account: Account }) {
  return (
    <div style={styles.accountEntry}>
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
  );
}

const styles: Record<string, React.CSSProperties> = {
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
  accountEntry: {
    border: "1px solid #cbd5e1",
    borderRadius: "10px",
    padding: "16px",
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    backgroundColor: "#ffffff",
  },
};