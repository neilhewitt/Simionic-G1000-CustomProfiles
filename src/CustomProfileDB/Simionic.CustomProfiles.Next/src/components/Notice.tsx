"use client";

import { useRouter } from "next/navigation";
import { useState } from "react";
import Link from "next/link";

interface NoticeProps {
  headline: string;
  strapLine?: string;
  showSearch?: boolean;
  showLinks?: boolean;
}

export default function Notice({ headline, strapLine, showSearch, showLinks }: NoticeProps) {
  const router = useRouter();
  const [searchTerm, setSearchTerm] = useState("");

  function doSearch() {
    if (searchTerm.trim()) {
      router.push(`/profiles?search=${encodeURIComponent(searchTerm)}`);
    }
  }

  return (
    <main className="bg-dark py-5">
      <div className="container px-5">
        <div className="row gx-5 align-items-center justify-content-center">
          <div className="col-lg-8 col-xl-7 col-xxl-6">
            <div className="my-5 text-center text-xl-start">
              <h1 className="display-5 fw-bolder text-white mb-2">{headline}</h1>
              {strapLine && (
                <p className="lead fw-normal text-white-50 mb-4">{strapLine}</p>
              )}
              {showSearch && (
                <div className="d-grid gap-3 d-sm-flex justify-content-sm-center justify-content-xl-start">
                  <input
                    className="px-4 rounded border-0 shadow-none form-control form-control-lg d-sm-none"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    onKeyDown={(e) => {
                      if (e.key === "Enter") doSearch();
                    }}
                    placeholder="Search profiles..."
                  />
                  <input
                    className="px-4 rounded border-0 shadow-none form-control form-control-lg w-50 d-none d-sm-inline"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    onKeyDown={(e) => {
                      if (e.key === "Enter") doSearch();
                    }}
                    placeholder="Search profiles..."
                  />
                  <button
                    className="btn btn-primary btn-lg px-4 me-sm-3 shadow-none d-inline"
                    onClick={doSearch}
                  >
                    Search
                  </button>
                </div>
              )}
            </div>
            {showLinks && (
              <div className="my-5 text-center text-xl-start pt-4">
                <p className="lead fw-normal text-white-50 mt-4">
                  You can <Link href="/profiles">browse profiles</Link>,{" "}
                  <Link href="/import">import</Link> or{" "}
                  <Link href="/create">create</Link> a profile,
                  <br />
                  <Link href="/downloads">download tools</Link>, or{" "}
                  <Link href="/faq">read frequently asked questions</Link>.
                </p>
              </div>
            )}
          </div>
          <div className="col-xl-5 col-xxl-6 d-none d-xl-block text-center">
            {/* eslint-disable-next-line @next/next/no-img-element */}
            <img
              className="img-fluid rounded-3 my-5"
              src="/img/G1000_screen.jpg"
              alt="G1000 MFD & PFD set into a cockpit instrument panel"
            />
          </div>
        </div>
      </div>
    </main>
  );
}
