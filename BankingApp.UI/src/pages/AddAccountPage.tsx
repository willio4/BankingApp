import { useNavigate } from "react-router-dom";
import { UserHeader } from "../components/UserHeader";
import { useAuth } from "../context/AuthContext";
import { useEffect } from "react";
import { api } from "../api/axiosClient";
import "./AddAccountPage.css";

export const AddAccountPage: React.FC = () => {
  const { user, isAuthenticated, refreshUser } = useAuth();
  const navigateTo = useNavigate();

  useEffect(() => {
    if (!isAuthenticated) {
      navigateTo("/login");
    }
  });

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const accountElement = document.querySelector(
      'input[name="accountType"]:checked',
    ) as HTMLInputElement | null;
    const currencyElement = document.getElementById(
      "currency",
    ) as HTMLSelectElement;
    const depositElement = document.getElementById(
      "deposit",
    ) as HTMLInputElement;

    if (!accountElement) {
      alert("Please select an account type");
      return;
    }

    const accountType: string = accountElement.value;
    const currency: string = currencyElement.value;
    const deposit: string = depositElement.value;

    try {
      const response = await api.post("/customer/open-account", {
        CustomerId: id,
        AccountType: parseInt(accountType, 10),
        Currency: currency,
      });

      if (deposit != null && deposit.trim() !== "") {
        const amount = parseFloat(deposit);
        try {
          await api.post("/transaction/deposit", {
            AccountNumber: response.data["accountNumber"],
            Amount: amount,
            Currency: currency,
          });
          await refreshUser();
          navigateTo("/my/dashboard");
          // eslint-disable-next-line @typescript-eslint/no-explicit-any
        } catch (error: any) {
          if (error.response) {
            const errorDetails =
              error.response.data.message ||
              error.response.data.error ||
              JSON.stringify(error.response.data);

            alert(`API Error Details: ${errorDetails}`);
          } else if (error.request) {
            alert("No response received from the server. Check your network.");
          } else {
            alert(`Request Setup Error: ${error.message}`);
          }
        }
      } else {
        await refreshUser();
        navigateTo("/my/dashboard");
      }
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (error: any) {
      if (error.response) {
        const errorDetails =
          error.response.data.message ||
          error.response.data.error ||
          JSON.stringify(error.response.data);

        alert(`API Error Details: ${errorDetails}`);
      } else if (error.request) {
        alert("No response received from the server. Check your network.");
      } else {
        alert(`Request Setup Error: ${error.message}`);
      }
    }
  };

  if (!user || !user.customer) {
    return null;
  }

  const { firstName, lastName, id } = user.customer;
  return (
    <div className="app-container">
      <UserHeader firstName={firstName} id={id} />

      <div className="page-content">
        <h3 className="page-content-title">Account Request Form</h3>

        <div className="form-card">
          <form className="add-account-form" onSubmit={handleSubmit}>
            <div className="form-field">
              <label htmlFor="firstName" className="form-label">
                First Name
              </label>
              <input
                id="firstName"
                className="form-input"
                value={firstName}
                disabled
              />
            </div>

            <div className="form-field">
              <label htmlFor="lastName" className="form-label">
                Last Name
              </label>
              <input
                id="lastName"
                className="form-input"
                value={lastName}
                disabled
              />
            </div>

            <div className="form-field">
              <label htmlFor="memberId" className="form-label">
                Apex Member ID
              </label>
              <input
                id="memberId"
                className="form-input"
                value={`*${id.slice(-6)}`}
                disabled
              />
            </div>

            <div className="form-field">
              <fieldset className="form-fieldset">
                <legend className="form-label">Account Type</legend>
                <div className="radio-group">
                  <label htmlFor="accountChecking" className="radio-option">
                    <input
                      type="radio"
                      name="accountType"
                      id="accountChecking"
                      value="0"
                      defaultChecked
                    />
                    Checking Account
                  </label>
                  <label htmlFor="accountSavings" className="radio-option">
                    <input
                      type="radio"
                      name="accountType"
                      id="accountSavings"
                      value="1"
                    />
                    Savings Account
                  </label>
                </div>
              </fieldset>
            </div>

            <div className="form-field">
              <label htmlFor="currency" className="form-label">
                Currency
              </label>
              <select id="currency" className="form-select">
                <option value="USD">USD</option>
                <option value="EUR">EUR</option>
                <option value="CAD">CAD</option>
                <option value="JPY">JPY</option>
              </select>
            </div>

            <div className="form-field">
              <label htmlFor="deposit" className="form-label">
                Initial Deposit (optional)
              </label>
              <input
                className="form-input"
                name="deposit"
                id="deposit"
                type="number"
                placeholder="0.00"
                min="0.01"
                step="0.01"
              />
            </div>

            <button type="submit" className="submit-button">
              Send Request
            </button>
          </form>
        </div>
      </div>
    </div>
  );
};
