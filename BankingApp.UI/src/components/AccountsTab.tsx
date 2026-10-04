import { faArrowRightArrowLeft, faCreditCard, faFileLines, faPlus } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import type { Account } from "../types/api";

export function AccountsTab({accounts} : {accounts:Account[]}) {


    return (
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
    );
}

const styles: Record<string, React.CSSProperties> = {
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
};