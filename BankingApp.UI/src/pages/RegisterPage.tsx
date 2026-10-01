import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { api } from "../api/axiosClient";

export const RegisterPage: React.FC = () => {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phoneNumber: "",
    password: "",
    confirmPassword: "",
    dateOfBirth: "",
    admin: false,
  });

  const [error, setError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isChecked, setIsChecked] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({
      ...formData,
      [e.target.name]:
        e.target.type === "checkbox"
          ? setIsChecked(e.target.checked)
          : e.target.value,
    });
  };

  const handleSubmit = async (e: React.SubmitEvent) => {
    e.preventDefault();
    setError(null);

    if (formData.password != formData.confirmPassword) {
      setError("Passwords do not match.");
      return;
    }

    setIsSubmitting(true);

    try {
      await api.post("/auth/register", {
        firstName: formData.firstName,
        lastName: formData.lastName,
        email: formData.email,
        phoneNumber: formData.phoneNumber,
        password: formData.password,
        confirmPassword: formData.confirmPassword,
        dateOfBirth: formData.dateOfBirth,
        admin: formData.admin ? 1 : 0,
      });

      navigate("/login", { state: { registered: true } });
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (err: any) {
      if (err.response?.data?.errors) {
        const messages = Object.values(err.response.data.errors).flat();
        setError(messages.join(" "));
      } else if (err.response?.data?.detail) {
        setError(err.response.data.detail);
      } else {
        setError(
          "Registration failed. Please check your details and try again.",
        );
      }
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div style={styles.container}>
      <div style={styles.card}>
        <div style={styles.headerGroup}>
          <h1 style={styles.createAccount}>Create Account</h1>
          <p style={styles.subtitle}>
            Register now to start banking with{" "}
            <span style={styles.bank}>ApexBank</span>
          </p>
        </div>
        {error && <div style={styles.errorBanner}>{error}</div>}

        <form onSubmit={handleSubmit} style={styles.form}>
          <div style={styles.row}>
            <div style={styles.field}>
              <label htmlFor="firstName" style={styles.label}>
                First Name
              </label>
              <input
                id="firstName"
                name="firstName"
                type="text"
                value={formData.firstName}
                onChange={handleChange}
                required
                style={styles.inputBox}
              />
            </div>
            <div style={styles.field}>
              <label htmlFor="lastName" style={styles.label}>
                Last Name
              </label>
              <input
                id="lastName"
                name="lastName"
                type="text"
                value={formData.lastName}
                onChange={handleChange}
                required
                style={styles.inputBox}
              />
            </div>
          </div>

          <div style={styles.field}>
            <label htmlFor="email" style={styles.label}>
              Email Address
            </label>
            <input
              id="email"
              name="email"
              type="email"
              value={formData.email}
              onChange={handleChange}
              required
              style={styles.inputBox}
            />
          </div>

          <div style={styles.field}>
            <label htmlFor="phoneNumber" style={styles.label}>
              Phone Number
            </label>
            <input
              id="phoneNumber"
              name="phoneNumber"
              type="tel"
              value={formData.phoneNumber}
              onChange={handleChange}
              required
              style={styles.inputBox}
            />
          </div>

          <div style={styles.field}>
            <label htmlFor="password" style={styles.label}>
              Password
            </label>
            <input
              id="password"
              name="password"
              type="password"
              value={formData.password}
              onChange={handleChange}
              required
              style={styles.inputBox}
            />
          </div>

          <div style={styles.field}>
            <label htmlFor="confirmPassword" style={styles.label}>
              Confirm Password
            </label>
            <input
              id="confirmPassword"
              name="confirmPassword"
              type="password"
              value={formData.confirmPassword}
              onChange={handleChange}
              required
              style={styles.inputBox}
            />
          </div>

          <div style={styles.field}>
            <label style={styles.label} htmlFor="dateOfBirth">
              Date of Birth
            </label>
            <input
              id="dateOfBirth"
              name="dateOfBirth"
              type="date"
              value={formData.dateOfBirth}
              onChange={handleChange}
              required
              style={styles.inputBox}
            />
          </div>

          <div style={styles.field}>
            <label htmlFor="userType" style={{ ...styles.label }}>
              Admin?
            </label>
            <input
              id="userType"
              name="userType"
              type="checkbox"
              checked={isChecked}
              onChange={handleChange}
              style={styles.checkbox}
            />
          </div>

          <button type="submit" disabled={isSubmitting} style={styles.button}>
            {isSubmitting ? "Creating Account" : "Register"}
          </button>

          <p style={styles.footerText}>
            Already have an account?
            <Link to={"/login"} style={styles.loginLink}>
              {" "}
              Login here
            </Link>
          </p>
        </form>
      </div>
    </div>
  );
};

const styles: Record<string, React.CSSProperties> = {
  container: {
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    minHeight: "calc(100vh - 120px)",
    backgroundColor: "#f8fafc", // Subtle slate backdrop
    padding: "20px",
    fontFamily: "system-ui, -apple-system, sans-serif",
    WebkitFontSmoothing: "antialiased",
  },
  card: {
    width: "100%",
    maxWidth: "420px",
    padding: "2.5rem 2.25rem",
    borderRadius: "12px",
    boxShadow:
      "0 10px 25px -5px rgba(0, 0, 0, 0.05), 0 8px 10px -6px rgba(0, 0, 0, 0.01)",
    backgroundColor: "#ffffff",
    border: "1px solid #e2e8f0",
  },
  form: {
    display: "flex",
    flexDirection: "column",
    gap: "1.25rem",
  },
  label: {
    fontSize: "14px",
    fontWeight: "500",
    color: "#334155",
  },
  row: {
    display: "grid",
    gridTemplateColumns: "1fr 1fr",
    gap: "1rem",
  },
  field: {
    display: "flex",
    flexDirection: "column",
    gap: "0.375rem",
  },
  button: {
    marginTop: "0.5rem",
    padding: "11px 16px",
    borderRadius: "8px",
    border: "none",
    backgroundColor: "#0d6e3d",
    color: "#ffffff",
    fontSize: "14px",
    fontWeight: "600",
    transition: "background-color 0.2s ease",
  },
  errorBanner: {
    backgroundColor: "#fef2f2",
    color: "#991b1b",
    border: "1px solid #fecaca",
    padding: "0.75rem 1rem",
    borderRadius: "8px",
    marginBottom: "1.25rem",
    fontSize: "14px",
  },
  footerText: {
    marginTop: "1.75rem",
    textAlign: "center",
    fontSize: "14px",
    color: "#64748b",
    margin: "1.75rem 0 0 0",
  },
  loginLink: {
    color: "#0d6e3d",
    fontWeight: "600",
    textDecoration: "none",
  },
  inputBox: {
    padding: "10px 14px",
    fontSize: "14px",
    border: "1px solid #cbd5e1",
    borderRadius: "8px",
    backgroundColor: "#ffffff",
    color: "#0f172a",
    outline: "none",
    boxSizing: "border-box",
    width: "100%",
    transition: "border-color 0.15s ease, box-shadow 0.15s ease",
  },

  headerGroup: {
    textAlign: "center",
    marginBottom: "1.75rem",
  },
  createAccount: {
    margin: 0,
    fontSize: "24px",
    fontWeight: "700",
    color: "#0f172a",
    letterSpacing: "-0.02em",
  },
  subtitle: {
    margin: "6px 0 0 0",
    fontSize: "14px",
    color: "#64748b",
  },
  bank: {
    // fontFamily: "initial",
    fontWeight: "bolder",
    letterSpacing: "-0.02em",
  },
  checkbox: {
    accentColor: "gold",
  },
};
