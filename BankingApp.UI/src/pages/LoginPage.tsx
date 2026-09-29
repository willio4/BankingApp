import React, { useState } from "react";
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

  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.SubmitEvent) => {
    e.preventDefault();
    setError(null);
    setIsSubmitting(true);

    try {
      const response = await api.post<LoginResponse>("/auth/login", {
        email,
        password,
      });

      const { refreshToken, ...authData } = response.data;
      login(authData, refreshToken);
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
        <h2>Sign In</h2>
        {error && <div style={styles.errorBanner}>{error}</div>}

        <form onSubmit={handleSubmit} style={styles.form}>
            <div style={styles.field}>
                <label htmlFor="email">Email Address</label>
                <input
                    id="email"
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                    placeholder="name@example.com"
                />
            </div>

            <div style={styles.field}>
                <label htmlFor="password">Password</label>
                <input
                    id="password"
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                    placeholder="••••••••"
                />
            </div>

            <button type="submit" disabled={isSubmitting} style={styles.button}>
                {isSubmitting ? 'Signing in...' : "Sign In"}
            </button>
        </form>

        <p style={styles.footerText}>
            Don't have an account? <Link to="/register">Register here.</Link>
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
    minHeight: "80vh",
  },
  card: {
    width: "100%",
    maxWidth: "400px",
    padding: "2rem",
    borderRadius: "8px",
    boxShadow: "0 4px 12px rgba(0,0,0,0.1)",
    backgroundColor: "#fff",
  },
  form: {
    display: "flex",
    flexDirection: "column",
    gap: "1.25rem",
  },
  field: {
    display: "flex",
    flexDirection: "column",
    gap: "0.5rem",
  },
  button: {
    padding: "0.75rem",
    borderRadius: "4px",
    border: "none",
    backgroundColor: "#0066cc",
    color: "#fff",
    fontWeight: "bold",
    cursor: "pointer",
  },
  errorBanner: {
    backgroundColor: "#ffebe9",
    color: "#cf222e",
    padding: "0.75rem",
    borderRadius: "4px",
    marginBottom: "1rem",
    fontSize: "0.9rem",
  },
  footerText: {
    marginTop: "1.5rem",
    textAlign: "center",
    fontSize: "0.9rem",
  },
};
