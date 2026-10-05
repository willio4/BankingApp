import { useNavigate } from "react-router-dom";
import { UserHeader } from "../components/UserHeader";
import { useAuth } from "../context/AuthContext";
import { useEffect } from "react";
import { api } from "../api/axiosClient";

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

    const accountType = accountElement.value;
    const currency = currencyElement.value;
    const deposit = depositElement.value;

    try {
      const response = await api.post("/customer/open-account", {
        CustomerId: id,
        AccountType: parseInt(accountType, 10),
        Currency: currency,
      });

      if (deposit != null || deposit.trim() != "") {
        const amount = parseFloat(deposit);
        try {
          await api.post("/transaction/deposit", {
            AccountNumber: response.data["accountNumber"],
            Amount: amount,
            Currency: currency,
          });
          await refreshUser();
          navigateTo("/my/dashboard");
        } catch (error) {
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
    } catch (error) {
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

  const { firstName, lastName, id } = user.customer;
  return (
    <div style={styles.container}>
      <UserHeader firstName={firstName} id={id} />
      <div style={styles.pageContent}>
        <form onSubmit={handleSubmit} style={styles.AddAccountForm}>
          <h3>Account Request Form</h3>
          <label>First Name</label>
          <input value={firstName} disabled />
          <label>Last Name</label>
          <input value={lastName} disabled />
          <label>Apex Member Id</label>
          <input value={`*${id.slice(-6)}`} disabled />
          <fieldset>
            <legend>Account Type</legend>
            <div>
              <input
                type="radio"
                name="accountType"
                id="accountType"
                value="0"
              />
              <label>Checking Account</label>
              <input
                type="radio"
                name="accountType"
                id="accountType"
                value="1"
              />
              <label>Savings Account</label>
            </div>
          </fieldset>
          <label>Currency</label>
          <select id="currency">
            <option value="USD">USD</option>
            <option value="EUR">EUR</option>
            <option value="CAD">CAD</option>
            <option value="JPY">JPY</option>
          </select>
          <label>Initial Deposit (optional)</label>
          <input name="deposit" id="deposit" />
          <button type="submit">Send Request</button>
        </form>
      </div>
    </div>
  );
};

const styles: Record<string, React.CSSProperties> = {
  container: {
    padding: "24px 32px",
    fontFamily: "system-ui, -apple-system, sans-serif",
    backgroundColor: "#f8fafc",
    minHeight: "100vh",
    boxSizing: "border-box",
  },
};
