"use client";

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import { useRouter, useParams, useSearchParams } from "next/navigation";
import Link from "next/link";
import { Profile } from "@/types";
import ProfileEditor from "@/components/ProfileEditor";
import { createDefaultProfile } from "@/lib/profile-utils";

export default function EditProfileContent() {
  const { data: session } = useSession();
  const router = useRouter();
  const params = useParams();
  const searchParams = useSearchParams();
  const id = params.id as string;
  const isNew = id === "new";

  const [profile, setProfile] = useState<Profile | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);
  const [saved, setSaved] = useState(false);
  const [showSaveConfirm, setShowSaveConfirm] = useState(false);

  const ownerId = (session as any)?.ownerId ?? null;
  const isLoggedIn = !!session?.user;

  useEffect(() => {
    if (!isLoggedIn) return;

    if (isNew) {
      const name = searchParams.get("name") ?? "New Profile";
      const newProfile = createDefaultProfile();
      newProfile.Name = name;
      newProfile.Owner = {
        Id: ownerId,
        Name: session?.user?.name ?? null,
      };
      setProfile(newProfile);
    } else {
      fetch(`/api/profiles/${id}`)
        .then((res) => {
          if (!res.ok) throw new Error("Failed to fetch profile");
          return res.json();
        })
        .then((data) => {
          if (data.Owner?.Id !== ownerId) {
            setError("You do not own this profile.");
            return;
          }
          setProfile(data);
        })
        .catch((err) => setError(err.message));
    }
  }, [id, isNew, isLoggedIn, ownerId, session?.user?.name, searchParams]);

  async function save() {
    if (!profile || saving) return;

    if (profile.id === null && !showSaveConfirm) {
      setShowSaveConfirm(true);
      return;
    }

    setShowSaveConfirm(false);
    setSaving(true);

    try {
      const profileId = profile.id ?? crypto.randomUUID();
      const profileToSave = { ...profile, id: profileId };

      const res = await fetch(`/api/profiles/${profileId}`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(profileToSave),
      });

      if (!res.ok) throw new Error("Failed to save profile");

      setProfile(profileToSave);
      setSaving(false);
      setSaved(true);

      setTimeout(() => setSaved(false), 2000);

      if (isNew) {
        router.replace(`/edit/${profileId}`);
      }
    } catch (err) {
      setSaving(false);
      setError(err instanceof Error ? err.message : "Failed to save");
    }
  }

  function getSaveButtonText() {
    if (saving) return "Saving changes...";
    if (saved) return "Changes saved";
    if (profile?.id === null) return "Save as draft";
    return "Save changes";
  }

  if (!isLoggedIn) {
    return (
      <section className="bg-white py-5 vh-100">
        <div className="text-center">
          <h3 className="fw-bolder">Please log in to edit profiles</h3>
        </div>
      </section>
    );
  }

  return (
    <section className="bg-white pt-5">
      <div className="text-center mb-2">
        <h3 className="fw-bolder">
          {profile?.Name ?? "Loading..."}
          {profile && (
            <span className="text-danger ms-3 fs-5">
              {profile.id === null ? "New Profile" : "Editing"}
            </span>
          )}
        </h3>
      </div>

      {profile && (
        <>
          <div className="text-center mb-4">
            <h5>By {profile.Owner?.Name}</h5>
          </div>

          <div className="container" style={{ maxWidth: "960px" }}>
            <div className="bg-light rounded-3 p-3 mb-5">
              {profile.id === null && (
                <div className="alert alert-danger">
                  <b>If you leave this page without saving your new profile as a draft, it will be lost.</b>
                </div>
              )}

              {profile.id !== null && (
                <div className="alert alert-danger">
                  <b>Changes made here do not take effect until you click &apos;save changes&apos;.</b>
                </div>
              )}

              {profile.id !== null && (
                <div className="d-flex align-items-center gap-3 mb-4">
                  <label className="fw-bold">Status</label>
                  <div className="btn-group">
                    <button
                      className={`btn btn-sm ${!profile.IsPublished ? "btn-primary" : "btn-secondary"}`}
                      onClick={() => setProfile({ ...profile, IsPublished: false })}
                    >
                      Draft
                    </button>
                    <button
                      className={`btn btn-sm ${profile.IsPublished ? "btn-primary" : "btn-secondary"}`}
                      onClick={() => setProfile({ ...profile, IsPublished: true })}
                    >
                      Published
                    </button>
                  </div>
                </div>
              )}

              <ProfileEditor
                profile={profile}
                editing={true}
                onChange={setProfile}
              />

              <div className="d-flex flex-wrap align-items-center gap-3 mt-4">
                <label className="fw-bold">
                  Profile name{" "}
                  <input
                    type="text"
                    className="form-control d-inline-block w-auto ms-2"
                    value={profile.Name}
                    onChange={(e) => setProfile({ ...profile, Name: e.target.value })}
                  />
                </label>
                <button
                  className={`btn btn-sm ${saving || saved ? "btn-secondary opacity-50" : "btn-primary"}`}
                  onClick={save}
                  disabled={saving || saved}
                >
                  {getSaveButtonText()}
                </button>
              </div>
            </div>
          </div>
        </>
      )}

      {showSaveConfirm && (
        <div className="modal d-block" style={{ backgroundColor: "rgba(0,0,0,0.5)" }}>
          <div className="modal-dialog modal-dialog-centered">
            <div className="modal-content">
              <div className="modal-body p-4">
                <p>This will save your new profile to the database as a draft</p>
                <div className="d-flex gap-2 justify-content-end">
                  <button className="btn btn-success" onClick={save}>Save</button>
                  <button className="btn btn-primary" onClick={() => { setShowSaveConfirm(false); setSaving(false); }}>Cancel</button>
                </div>
              </div>
            </div>
          </div>
        </div>
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
