import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function DashboardPage() {
  const { user, isAuthenticated } = useAuth()
  const navigate = useNavigate();

  return (
    <>
      {!isAuthenticated && navigate("/login")}
      <div style={styles.heading}>
        <h2 style={styles.greeting}>Good Evening, {user?.customer.firstName}</h2>
        <h3></h3>
      </div>

      <div style={styles.container}>
        <div>Grid 1</div>
        <div>Grid 2</div>
      </div>
    </>
  );
}

const styles: Record<string, React.CSSProperties> = {
  container: {
    display: "grid",
    minHeight: "100vh",
    gridTemplateColumns: "1fr 1fr",
    padding: "30px"
  },
  greeting: {
    margin: "100px",
    fontSize: "30px",
    justifyContent: "flex-start"
  },
  // heading: {
  //   // display: "flex"
  // }
}
