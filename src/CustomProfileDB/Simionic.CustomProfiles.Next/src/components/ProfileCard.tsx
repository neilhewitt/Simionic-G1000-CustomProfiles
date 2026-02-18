"use client";

import { useRouter } from "next/navigation";
import { ProfileSummary } from "@/types";
import { getAircraftTypeImage, getAircraftTypeName } from "@/lib/profile-utils";

interface ProfileCardProps {
  profile: ProfileSummary;
  isOwner: boolean;
}

export default function ProfileCard({ profile, isOwner }: ProfileCardProps) {
  const router = useRouter();

  return (
    <div
      className="col-lg-4 mb-5"
      style={{ cursor: "pointer" }}
      onClick={() => router.push(`/profile/${profile.id}`)}
    >
      <div className={`card h-100 border-1 shadow-sm ${profile.IsPublished ? "" : "bg-mid"}`}>
        {/* eslint-disable-next-line @next/next/no-img-element */}
        <img
          className="card-img-top"
          src={getAircraftTypeImage(profile.AircraftType)}
          alt="Simionic custom profile screen"
        />
        <div className="card-body p-4">
          <div className="container p-0 m-0">
            <div className="row p-0 m-0">
              <div className="col-8 p-0">
                <h5 className="card-title mb-3">{profile.Name}</h5>
              </div>
              <div className="col-4 p-0">
                {isOwner && (
                  <span>
                    <button
                      className="btn-sm btn-primary float-end border-0 shadow-none me-2"
                      onClick={(e) => {
                        e.stopPropagation();
                        router.push(`/edit/${profile.id}`);
                      }}
                    >
                      Edit
                    </button>
                  </span>
                )}
              </div>
            </div>
          </div>
          <div>
            <p className="card-text mb-0">
              <b>Aircraft type:</b> {profile.Engines === 2 ? "Twin " : "Single "}
              {getAircraftTypeName(profile.AircraftType)}
            </p>
          </div>
        </div>
        <div className="card-footer p-4 pt-0 bg-transparent border-top-0">
          <div className="d-flex align-items-end justify-content-between">
            <div className="d-flex align-items-center">
              <div className="small">
                <div className="text">
                  <b>By:</b> {profile.Owner?.Name ?? "Unknown"}
                  {isOwner && !profile.IsPublished && (
                    <>
                      <span className="text-black">&nbsp;|&nbsp;</span>
                      <span className="text-black-50 fw-bold">Draft</span>
                    </>
                  )}
                </div>
                <div className="text float-start">
                  <b>Updated:</b> {new Date(profile.LastUpdated).toLocaleDateString()}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
