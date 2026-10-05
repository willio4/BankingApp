import {
  faArrowRightArrowLeft,
  faPaperPlane,
  faFileLines,
  faPlus,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import type { Account as AccountType } from "../types/api";
import { Account } from "./Account";
import { useNavigate } from "react-router-dom";
import "./AccountsTab.css";
interface AccountsTabProps {
  accounts: AccountType[];
}

export function AccountsTab({ accounts }: AccountsTabProps) {
  const navigate = useNavigate();

  const handleAddAccountClick = () => {
    navigate("/my/add-account");
  };

  const hasAccounts = accounts && accounts.length > 0;

  return (
    <section className="accounts-card">
      <header className="accounts-header">
        <h3 className="accounts-title">Accounts</h3>

        <nav aria-label="Account actions">
          <div className="accounts-actions">
            <button type="button" className="account-action-btn">
              <FontAwesomeIcon icon={faArrowRightArrowLeft} />
              <span>Transfer</span>
            </button>
            <button type="button" className="account-action-btn">
              <FontAwesomeIcon icon={faPaperPlane} />
              <span>Send money</span>
            </button>
            <button type="button" className="account-action-btn">
              <FontAwesomeIcon icon={faFileLines} />
              <span>Statements</span>
            </button>
            <button
              type="button"
              className="account-action-btn"
              onClick={handleAddAccountClick}
            >
              <FontAwesomeIcon icon={faPlus} />
              <span>Add account</span>
            </button>
          </div>
        </nav>
      </header>

      <div className="accounts-list">
        {hasAccounts ? (
          accounts.map((account) => (
            <Account key={account.id} account={account} />
          ))
        ) : (
          <div className="accounts-empty-state">
            <p>No active accounts found.</p>
            <button
              type="button"
              className="btn-add-first-account"
              onClick={handleAddAccountClick}
            >
              <FontAwesomeIcon icon={faPlus} />
              <span>Open an Account</span>
            </button>
          </div>
        )}
      </div>
    </section>
  );
}
