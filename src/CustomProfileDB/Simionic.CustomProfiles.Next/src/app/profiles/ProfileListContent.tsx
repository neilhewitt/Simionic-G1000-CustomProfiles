"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { useSearchParams } from "next/navigation";
import { ProfileSummary, AircraftType, PublishedStatus } from "@/types";
import { filterProfiles } from "@/lib/profile-utils";
import ProfileCard from "@/components/ProfileCard";
import ProfileFilters from "@/components/ProfileFilters";

export default function ProfileListContent() {
  const { data: session } = useSession();
  const searchParams = useSearchParams();

  const [profiles, setProfiles] = useState<ProfileSummary[] | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [searchTerm, setSearchTerm] = useState(searchParams.get("search") ?? "");
  const [typeFilter, setTypeFilter] = useState<AircraftType | null>(null);
  const [engineFilter, setEngineFilter] = useState<number | null>(null);
  const [onlyShowMine, setOnlyShowMine] = useState(false);
  const [onlyShowDrafts, setOnlyShowDrafts] = useState(false);

  const isLoggedIn = !!session?.user;
  const ownerId = (session as any)?.ownerId ?? null;

  useEffect(() => {
    fetchProfiles();
  }, []);

  async function fetchProfiles() {
    try {
      const res = await fetch("/api/profiles");
      if (!res.ok) throw new Error("Failed to fetch profiles");
      const data = await res.json();
      setProfiles(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Unknown error");
    }
  }

  function getFiltered(): ProfileSummary[] {
    if (!profiles) return [];

    const published = (() => {
      if (isLoggedIn && onlyShowDrafts) return PublishedStatus.UnpublishedOwner;
      if (isLoggedIn) return PublishedStatus.PublishedOwner;
      return PublishedStatus.Published;
    })();

    return filterProfiles(
      profiles,
      published,
      typeFilter,
      engineFilter,
      ownerId,
      onlyShowMine,
      searchTerm || null
    );
  }

  function reset() {
    setSearchTerm("");
    setTypeFilter(null);
    setEngineFilter(null);
    setOnlyShowMine(false);
    setOnlyShowDrafts(false);
  }

  const filtered = getFiltered();

  const title = (() => {
    if (isLoggedIn && onlyShowMine) {
      return `${session?.user?.name}'s ${onlyShowDrafts ? "drafts" : "profiles"}`;
    }
    if (searchTerm) return "Search results";
    return "Browse profiles";
  })();

  return (
    <section className={`bg-white py-5 ${!profiles?.length ? "vh-100" : ""}`}>
      <div className="container px-5 my-3">
        <div className="row gx-5 justify-content-center">
          <div className="col-md-auto">
            <div className="text-center">
              <h3 className="fw-bolder">{title}</h3>
              <ProfileFilters
                searchTerm={searchTerm}
                onSearchChange={setSearchTerm}
                typeFilter={typeFilter}
                onTypeChange={setTypeFilter}
                engineFilter={engineFilter}
                onEngineChange={setEngineFilter}
                isLoggedIn={isLoggedIn}
                onlyShowMine={onlyShowMine}
                onlyShowDrafts={onlyShowDrafts}
                onOwnerFilterChange={(mine, drafts) => {
                  setOnlyShowMine(mine);
                  setOnlyShowDrafts(drafts);
                }}
                onReset={reset}
              />
            </div>
          </div>
        </div>

        {profiles ? (
          <>
            <div className="row gx-5">
              {filtered.map((profile) => (
                <ProfileCard
                  key={profile.id}
                  profile={profile}
                  isOwner={isLoggedIn && ownerId === profile.Owner?.Id}
                />
              ))}
            </div>
            {filtered.length === 0 && (
              <div className="text-center">
                <h5>Nothing to see here... try changing the filter.</h5>
              </div>
            )}
          </>
        ) : (
          <div className="text-center">
            <h5>Loading...</h5>
            {error && (
              <>
                <p className="text-danger">{error}</p>
                <p>Try again later, or if this persists, contact <a href="/contact">the site admin</a></p>
              </>
            )}
          </div>
        )}
      </div>
    </section>
  );
}
