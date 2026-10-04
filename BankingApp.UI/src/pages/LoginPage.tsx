import React, { useEffect, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { api } from "../api/axiosClient";
import type { AuthResponse } from "../types/api";

interface LoginResponse extends AuthResponse {
  refreshToken: string;
}

export const LoginPage: React.FC = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const { login, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if(isAuthenticated){
      navigate("/dashboard")
    }
  })

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setError(null);
    setIsSubmitting(true);

    try {
      const response = await api.post<LoginResponse>("/auth/login", {
        email,
        password,
      });

      const authData = response.data;
      login(authData, authData.refreshToken);
      navigate("/dashboard");
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (err: any) {
      if (err.response?.data?.title) {
        setError(err.response.data.title);
      } else if (err.response?.data?.message) {
        setError(err.response.data.message);
      } else {
        setError("Invalid email or password. Please try again.");
      }
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div style={styles.container}>
      <div style={styles.card}>
        <div style={styles.headerGroup}>
          <h1 style={styles.signIn}>Sign In</h1>
          <p style={styles.subtitle}>Access your online banking account</p>
        </div>

        {error && (
          <div style={styles.errorBanner} role="alert">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} style={styles.form}>
          <div style={styles.field}>
            <label htmlFor="email" style={styles.label}>
              Email Address
            </label>
            <input
              id="email"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              autoComplete="email"
              placeholder="name@example.com"
              style={styles.inputBox}
            />
          </div>

          <div style={styles.field}>
            <div style={styles.labelRow}>
              <label htmlFor="password" style={styles.label}>
                Password
              </label>
              <Link to="/forgot-password" style={styles.forgotLink}>
                Forgot password?
              </Link>
            </div>
            <input
              id="password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              autoComplete="current-password"
              placeholder="••••••••"
              style={styles.inputBox}
            />
          </div>

          <button
            type="submit"
            disabled={isSubmitting}
            style={{
              ...styles.button,
              opacity: isSubmitting ? 0.7 : 1,
              cursor: isSubmitting ? "not-allowed" : "pointer",
            }}
          >
            {isSubmitting ? "Signing in..." : "Sign In"}
          </button>
        </form>

        <p style={styles.footerText}>
          Don't have an account?{" "}
          <Link to="/register" style={styles.registerLink}>
            Register here
          </Link>
        </p>
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
    backgroundColor: "#f8fafc",
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
  headerGroup: {
    textAlign: "center",
    marginBottom: "1.75rem",
  },
  signIn: {
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
  form: {
    display: "flex",
    flexDirection: "column",
    gap: "1.25rem",
  },
  field: {
    display: "flex",
    flexDirection: "column",
    gap: "0.375rem",
  },
  labelRow: {
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
  },
  label: {
    fontSize: "14px",
    fontWeight: "500",
    color: "#334155",
  },
  forgotLink: {
    fontSize: "13px",
    color: "#0d6e3d",
    textDecoration: "none",
    fontWeight: "500",
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
    textAlign: "center",
    fontSize: "14px",
    color: "#64748b",
    margin: "1.75rem 0 0 0",
  },
  registerLink: {
    color: "#0d6e3d",
    fontWeight: "600",
    textDecoration: "none",
  },
};