import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faInbox,
  faPhone,
  faFileLines,
} from "@fortawesome/free-solid-svg-icons";
import "./UserHeader.css";

interface UserHeaderProps {
  firstName: string;
  id?: string;
}

const getGreeting = (): string => {
  const hour = new Date().getHours();
  if (hour < 12) return "Good Morning";
  if (hour < 18) return "Good Afternoon";
  return "Good Evening";
};

export function UserHeader({ firstName, id }: UserHeaderProps) {
  const greetingText = getGreeting();
  const maskedId = id ? `*${id.slice(-6)}` : "N/A";

  return (
    <header className="user-header">
      <div className="greeting-group">
        <h2 className="greeting-title">
          {greetingText}, {firstName}
        </h2>
        <span className="member-id">Member: {maskedId}</span>
      </div>

      <nav aria-label="Header shortcuts">
        <ul className="header-shortcuts">
          <li>
            <button type="button" className="shortcut-button">
              <FontAwesomeIcon icon={faInbox} />
              <span>Inbox</span>
            </button>
          </li>
          <li>
            <button type="button" className="shortcut-button">
              <FontAwesomeIcon icon={faFileLines} />
              <span>Documents</span>
            </button>
          </li>
          <li>
            <button type="button" className="shortcut-button">
              <FontAwesomeIcon icon={faPhone} />
              <span>Help</span>
            </button>
          </li>
        </ul>
      </nav>
    </header>
  );
}
