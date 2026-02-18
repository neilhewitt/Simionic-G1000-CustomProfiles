"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { useRouter, useParams } from "next/navigation";
import Link from "next/link";
import { Profile } from "@/types";
import ProfileEditor from "@/components/ProfileEditor";
import { exportProfileAsJson } from "@/lib/export";

export default function ProfileViewPage() {
  const { data: session } = useSession();
  const router = useRouter();
  const params = useParams();
  const id = params.id as string;

  const [profile, setProfile] = useState<Profile | null>(null);
  const [error, setError] = useState<string | null>(null);

  const ownerId = (session as any)?.ownerId ?? null;
  const canEdit = !!session?.user && !!ownerId && profile?.Owner?.Id === ownerId;

  useEffect(() => {
    if (id) {
      fetch(`/api/profiles/${id}`)
        .then((res) => {
          if (!res.ok) throw new Error("Failed to fetch profile");
          return res.json();
        })
        .then(setProfile)
        .catch((err) => setError(err.message));
    }
  }, [id]);

  return (
    <section className="bg-white pt-5">
      <div className="text-center mb-2">
        <h3 className="fw-bolder">
          {profile?.Name ?? "Loading..."}
        </h3>
      </div>

      {profile && (
        <>
          <div className="text-center mb-4">
            <h5>By {profile.Owner?.Name}</h5>
          </div>

          <div className="container" style={{ maxWidth: "960px" }}>
            <div className="bg-light rounded-3 p-3 mb-5">
              {profile.id && (
                <div className="d-flex justify-content-center gap-2 mb-5 bg-white rounded py-2">
                  {canEdit && (
                    <button
                      className="btn btn-primary btn-sm"
                      onClick={() => router.push(`/edit/${id}`)}
                    >
                      Edit
                    </button>
                  )}
                  <button
                    className="btn btn-success btn-sm"
                    onClick={() => exportProfileAsJson(profile)}
                  >
                    Export
                  </button>
                </div>
              )}

              {profile.Notes && (
                <div className="alert alert-light border">
                  <b>Author note:</b> {profile.Notes}
                </div>
              )}

              <ProfileEditor profile={profile} editing={false} />

              <div className="mt-4">
                <label className="fw-bold">
                  Profile name{" "}
                  <input
                    type="text"
                    className="form-control d-inline-block w-auto ms-2"
                    value={profile.Name}
                    disabled
                  />
                </label>
              </div>
            </div>
          </div>
        </>
      )}

      {error && (
        <>
          <p className="text-center text-danger">{error}</p>
          <p className="text-center">
            Please try again later, or if this persists, contact{" "}
            <Link href="/contact">the site admin</Link>.
          </p>
        </>
      )}

      <p className="text-center py-4">
        <Link href="/profiles">Back to profile list</Link>
      </p>
    </section>
  );
}
