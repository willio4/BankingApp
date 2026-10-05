import type { Account as AccountType } from "../types/api";
import "./Account.css";
interface AccountProps {
  account: AccountType;
  onDeposit?: (account: AccountType) => void;
  onWithdraw?: (account: AccountType) => void;
}

const onDeposit = () => {

}

const onWithdraw = () => {
  
}

export function Account({ account }: AccountProps) {
  const maskedNumber = account.accountNumber
    ? `*${account.accountNumber.slice(-4)}`
    : "****";

  const formattedBalance = new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: account.currency || "USD",
    minimumFractionDigits: 2,
  }).format(account.balance ?? 0);

  const accountTitle = account.type
    ? `ApexBank ${account.type}`
    : "ApexBank Account";

  return (
    <article className="account-entry">
      <div className="account-info">
        <span className="account-name">{accountTitle}</span>
        <span className="account-number">{maskedNumber}</span>
      </div>

      <div className="account-meta">
        <span className="account-balance">{formattedBalance}</span>
        <button
          type="button"
          className="account-action-link"
          onClick={onDeposit}
        >
          Deposit
        </button>
        <button
          type="button"
          className="account-action-link"
          onClick={onWithdraw}
        >
          Withdraw
        </button>
      </div>
    </article>
  );
}
