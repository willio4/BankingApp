import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function DashboardPage() {
  const { user, isAuthenticated } = useAuth()
  const navigate = useNavigate();

  return (
    <>
      {!isAuthenticated && navigate("/login")} 
      <h1>Dashboard Page</h1>
      <h2>Hello, {user.customer.firstName}</h2>
    </>
  );
}
