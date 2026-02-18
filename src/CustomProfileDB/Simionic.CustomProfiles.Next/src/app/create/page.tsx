"use client";

import { useState } from "react";
import { useSession, signIn } from "next-auth/react";
import { useRouter } from "next/navigation";
import Link from "next/link";

export default function CreatePage() {
  const { data: session } = useSession();
  const router = useRouter();
  const [profileName, setProfileName] = useState("");

  const isLoggedIn = !!session?.user;

  function createProfile() {
    if (profileName.trim()) {
      router.push(`/edit/new?name=${encodeURIComponent(profileName)}`);
    }
  }

  return (
    <main className="bg-dark py-5">
      <div className="container px-5">
        <div className="row gx-5 align-items-center justify-content-center">
          <div className="col-lg-8 col-xl-7 col-xxl-6">
            <div className="my-5 text-center text-xl-start">
              <h3 className="fw-bolder text-white mb-2">Create a profile</h3>
              {!isLoggedIn ? (
                <>
                  <p className="lead fw-normal text-white-50 mb-4">
                    To create a profile, you must be logged in with a Microsoft Account. You can log in{" "}
                    <a href="#" onClick={(e) => { e.preventDefault(); signIn("azure-ad"); }}>here</a>.
                  </p>
                  <p className="fw-normal text-white-50 mb-4">
                    You will be asked to give this site permissions to see your name and email address the first time you log in. You can find out about how we use your information{" "}
                    <Link href="/privacy">here</Link>.
                  </p>
                </>
              ) : (
                <>
                  <p className="lead fw-normal text-white-50">Choose a name for your new profile</p>
                  <div className="d-grid gap-3 d-sm-flex justify-content-sm-center justify-content-xl-start mb-5">
                    <input
                      className="px-4 rounded border-0 shadow-none form-control form-control-lg"
                      placeholder="Your new profile's name"
                      value={profileName}
                      onChange={(e) => setProfileName(e.target.value)}
                      onKeyDown={(e) => {
                        if (e.key === "Enter") createProfile();
                      }}
                    />
                    <button
                      className="btn btn-primary btn-lg px-4 me-sm-3"
                      onClick={createProfile}
                    >
                      Create
                    </button>
                  </div>
                  <p className="lead fw-normal text-white-50 mb-4">
                    This creates <b>a new empty profile</b> that you must fill out manually,
                    <br />but you can also <Link href="/import">import</Link> a profile that you have exported from your iPad.
                  </p>
                </>
              )}
            </div>
          </div>
          <div className="col-xl-5 col-xxl-6 d-none d-xl-block text-center">
            {/* eslint-disable-next-line @next/next/no-img-element */}
            <img className="img-fluid rounded-3 my-5" src="/img/G1000_screen.jpg" alt="G1000 MFD & PFD set into a cockpit instrument panel" />
          </div>
        </div>
      </div>
    </main>
  );
}
