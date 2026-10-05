import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import { ProtectedRoute } from "./routes/ProtectedRoute";
import { NavigationBar } from "./components/NavigationBar";

import { LoginPage } from "./pages/LoginPage";
import { RegisterPage } from "./pages/RegisterPage";
import DashboardPage from "./pages/DashboardPage";
import { AddAccountPage } from "./pages/AddAccountPage";

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <NavigationBar />
        <Routes>
          {/* Public Routes */}
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />

          {/* Protected Banking Routes */}
          <Route element={<ProtectedRoute />}>
            <Route path="/my/dashboard" element={<DashboardPage />} />
            <Route path="/my/add-account" element={<AddAccountPage />} />
          </Route>

          {/* Fallback */}
          <Route path="*" element={<Navigate to="/my/dashboard" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}
