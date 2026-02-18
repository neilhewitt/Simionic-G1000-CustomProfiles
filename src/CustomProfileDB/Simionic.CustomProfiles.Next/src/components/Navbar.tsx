"use client";

import Link from "next/link";
import { useSession, signIn, signOut } from "next-auth/react";
import { useState } from "react";

export default function Navbar() {
  const { data: session } = useSession();
  const [showUserMenu, setShowUserMenu] = useState(false);

  return (
    <header className="flex-shrink-0">
      <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
        <div className="container px-5">
          <Link href="/" className="navbar-brand p-0">
            <span className="d-none d-lg-inline">Simionic </span>G1000 Profile DB
          </Link>
          <div className="navbar navbar-expand">
            <ul className="navbar-nav ms-auto mb-lg-0">
              <li className="nav-item">
                <Link href="/profiles" className="nav-link pointer">
                  Browse<span className="d-none d-lg-inline"> profiles</span>
                </Link>
              </li>
              <li className="nav-item">
                <Link href="/import" className="nav-link">
                  Import<span className="d-none d-lg-inline"> a profile</span>
                </Link>
              </li>
              <li className="nav-item">
                <Link href="/create" className="nav-link">
                  Create<span className="d-none d-lg-inline"> a profile</span>
                </Link>
              </li>
              <li className="nav-item d-none d-lg-inline">
                <Link href="/downloads" className="nav-link">
                  Downloads
                </Link>
              </li>
              {session ? (
                <div className="dropdown show">
                  <button
                    className="nav-link pt-0 text-white bg-transparent border-0 cursor-pointer"
                    style={{ fontSize: "1.75rem" }}
                    onClick={() => setShowUserMenu(!showUserMenu)}
                    aria-label="User menu"
                  >
                    <span className="bi-person-fill" />
                  </button>
                  {showUserMenu && (
                    <div className="dropdown-menu show" style={{ display: "block", right: 0, left: "auto" }}>
                      <h5 className="dropdown-item-text mb-0">{session.user?.name}</h5>
                      <p className="dropdown-item-text text-muted mb-0">{session.user?.email}</p>
                      <button
                        onClick={() => signOut()}
                        className="dropdown-item text-primary bg-transparent border-0 cursor-pointer"
                      >
                        Log out
                      </button>
                    </div>
                  )}
                </div>
              ) : (
                <li className="nav-item">
                  <button
                    onClick={() => signIn("azure-ad")}
                    className="nav-link pointer text-white bg-transparent border-0 cursor-pointer"
                  >
                    Log<span className="d-none d-lg-inline">&nbsp;</span>in
                  </button>
                </li>
              )}
            </ul>
          </div>
        </div>
      </nav>
    </header>
  );
}
