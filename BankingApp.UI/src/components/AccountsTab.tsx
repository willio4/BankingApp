import {
  faArrowRightArrowLeft,
  faPaperPlane,
  faFileLines,
  faPlus,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import type { Account as AccountType } from "../types/api";
import { Account } from "./Account";

export function AccountsTab({ accounts }: { accounts: AccountType[] }) {
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
              <FontAwesomeIcon icon={faPaperPlane} />
              <span>Send money</span>
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
            <div key={account.id}>
              <Account account={account} />
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
  
};
