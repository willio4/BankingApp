import React, {
  createContext,
  useContext,
  useState,
  type ReactNode,
  useCallback,
} from "react";
import { type AuthResponse, type CustomerProfile } from "../types/api";
import { api } from "../api/axiosClient";

interface AuthContextType {
  user: AuthResponse | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (data: AuthResponse) => void;
  logout: () => void;
  refreshUser: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

// Helper function to synchronously initialize user state on mount
const getInitialUser = (): AuthResponse | null => {
  const accessToken = localStorage.getItem("accessToken");
  const refreshToken = localStorage.getItem("refreshToken");
  const storedCustomer = localStorage.getItem("user_profile");

  if (accessToken && refreshToken && storedCustomer) {
    try {
      return {
        accessToken,
        refreshToken,
        customer: JSON.parse(storedCustomer) as CustomerProfile,
      };
    } catch {
      localStorage.clear();
      return null;
    }
  }
  return null;
};

export const AuthProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const [user, setUser] = useState<AuthResponse | null>(getInitialUser);
  const [isLoading] = useState<boolean>(false);

  const login = (authData: AuthResponse) => {
    localStorage.setItem("accessToken", authData.accessToken);
    localStorage.setItem("refreshToken", authData.refreshToken);
    localStorage.setItem("user_profile", JSON.stringify(authData.customer));
    setUser(authData);
  };

  const logout = async () => {
    try {
      const refreshToken = localStorage.getItem("refreshToken");
      if (refreshToken) {
        await api.post("auth/logout", {
          RefreshToken: refreshToken,
        });
      }
    } catch {
      // intentionally left blank
    } finally {
      localStorage.clear();
      window.location.href = "/login";
    }
  };

  const refreshUser = useCallback(async () => {
    try {
      const storedCustomer = localStorage.getItem("user_profile");
      if (!storedCustomer) return;

      const parsedCustomer: CustomerProfile = JSON.parse(storedCustomer);

      // Fetch fresh customer profile from backend
      const response = await api.get<CustomerProfile>(
        `/customer/${parsedCustomer.id}`,
      );

      // Update state with new object reference
      setUser((prevUser) => {
        if (!prevUser) return null;

        const updatedUser: AuthResponse = {
          ...prevUser,
          customer: response.data,
        };

        // Save updated CustomerProfile back to localStorage
        localStorage.setItem("user_profile", JSON.stringify(response.data));

        return updatedUser;
      });
    } catch (error) {
      console.error("Failed to refresh user profile:", error);
    }
  }, []);

  return (
    <AuthContext.Provider
      value={{
        user,
        isAuthenticated: !!user,
        isLoading,
        login,
        logout,
        refreshUser,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

// eslint-disable-next-line react-refresh/only-export-components
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
