import React from "react";
import { Link, useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faBuildingColumns,
  faMagnifyingGlass,
  faCaretDown,
} from "@fortawesome/free-solid-svg-icons";
import { faMessage, faUserCircle } from "@fortawesome/free-regular-svg-icons";
import { useAuth } from "../context/AuthContext";
import "./NavigationBar.css";

export const NavigationBar: React.FC = () => {
  const { user, logout, isLoading } = useAuth();
  const navigateTo = useNavigate();

  if (isLoading) {
    return (
      <div className="navbar-loading">
        <span>Loading...</span>
      </div>
    );
  }

  const handleBrandClick = () => {
    navigateTo("/my/dashboard");
  };

  return (
    <header className="navbar">
      <button
        type="button"
        className="navbar-brand"
        onClick={handleBrandClick}
        aria-label="ApexBank Home"
      >
        <FontAwesomeIcon icon={faBuildingColumns} size="lg" />
        <span className="brand-name">ApexBank</span>
      </button>

      <nav className="navbar-nav" aria-label="Main Navigation">
        <ul className="nav-tabs">
          <li>
            <button type="button" className="nav-tab-button">
              Insurance
            </button>
          </li>
          <li>
            <button
              type="button"
              className="nav-tab-button active"
              aria-current="page"
            >
              Banking
            </button>
          </li>
          <li>
            <button type="button" className="nav-tab-button">
              Investing
            </button>
          </li>
          <li>
            <button type="button" className="nav-tab-button">
              Life Insurance
            </button>
          </li>
          <li>
            <button type="button" className="nav-tab-button">
              Advice
            </button>
          </li>
          <li>
            <button type="button" className="nav-tab-button">
              Membership
            </button>
          </li>
        </ul>
      </nav>

      <div className="navbar-tools">
        <ul className="tools-list">
          <li>
            <button type="button" className="tool-button">
              <FontAwesomeIcon icon={faMagnifyingGlass} />
              <span>Search</span>
            </button>
          </li>
          <li>
            <button type="button" className="tool-button">
              <FontAwesomeIcon icon={faMessage} />
              <span>Chat 24/7</span>
            </button>
          </li>
          <li>
            {user ? (
              <div className="user-actions">
                <button type="button" className="btn-primary-pill">
                  <FontAwesomeIcon icon={faUserCircle} />
                  <span>{user?.customer?.firstName}</span>
                  <FontAwesomeIcon icon={faCaretDown} />
                </button>
                <button type="button" onClick={logout} className="btn-logout">
                  Log Out
                </button>
              </div>
            ) : (
              <Link to="/login" className="btn-primary-pill">
                <FontAwesomeIcon icon={faUserCircle} />
                <span>Log On</span>
              </Link>
            )}
          </li>
        </ul>
      </div>
    </header>
  );
};
