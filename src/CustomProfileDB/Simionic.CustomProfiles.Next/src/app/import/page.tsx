"use client";

import { useState } from "react";
import { useSession, signIn } from "next-auth/react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { Profile } from "@/types";

export default function ImportPage() {
  const { data: session } = useSession();
  const router = useRouter();
  const [fileChosen, setFileChosen] = useState(false);
  const [uploading, setUploading] = useState(false);
  const [error, setError] = useState(false);
  const [fileName, setFileName] = useState("");

  const isLoggedIn = !!session?.user;
  const ownerId = (session as any)?.ownerId ?? null;

  async function handleFile(e: React.ChangeEvent<HTMLInputElement>) {
    const file = e.target.files?.[0];
    if (!file) return;

    setFileName(file.name);
    setFileChosen(true);
    setUploading(true);

    try {
      const text = await file.text();
      const profile: Profile = JSON.parse(text);

      const newId = crypto.randomUUID();
      profile.id = newId;
      profile.Owner = {
        Id: ownerId,
        Name: session?.user?.name ?? null,
      };
      profile.IsPublished = false;

      const res = await fetch(`/api/profiles/${newId}`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(profile),
      });

      if (!res.ok) throw new Error("Failed to upload profile");

      setUploading(false);
      await new Promise((resolve) => setTimeout(resolve, 1500));
      router.push(`/edit/${newId}`);
    } catch {
      setError(true);
      setUploading(false);
    }
  }

  function resetState() {
    setError(false);
    setUploading(false);
    setFileChosen(false);
  }

  return (
    <main className="bg-dark py-5">
      <div className="container px-5">
        <div className="row gx-5 align-items-center justify-content-center">
          <div className="col-lg-8 col-xl-7 col-xxl-6">
            <div className="my-5 text-center text-xl-start">
              <h3 className="fw-bolder text-white mb-2">Import a profile</h3>
              {!isLoggedIn ? (
                <>
                  <p className="lead fw-normal text-white-50 mb-4">
                    To import a profile, you must be logged in with a Microsoft Account. You can log in{" "}
                    <a href="#" onClick={(e) => { e.preventDefault(); signIn("azure-ad"); }}>here</a>.
                  </p>
                  <p className="fw-normal text-white-50 mb-4">
                    You will be asked to give this site permissions to see your name and email address the first time you log in. You can find out about how we use your information{" "}
                    <Link href="/privacy">here</Link>.
                  </p>
                </>
              ) : (
                <>
                  <p className="lead fw-normal text-white-50">Choose the exported profile (.json file) to import</p>
                  <div className="pb-5">
                    {!fileChosen && !error && (
                      <input
                        type="file"
                        accept=".json"
                        onChange={handleFile}
                        className="form-control form-control-lg"
                      />
                    )}
                    {fileChosen && !error && uploading && (
                      <h4 className="text-white">Importing &apos;{fileName}&apos;...</h4>
                    )}
                    {fileChosen && !error && !uploading && (
                      <h4 className="text-success">Imported successfully. Opening editor...</h4>
                    )}
                    {error && (
                      <>
                        <p className="lead fw-normal text-danger">An error occurred uploading the file. Are you sure this is a valid profile JSON file?</p>
                        <p className="lead fw-normal text-white-50 mb-4">
                          You can{" "}
                          <a href="#" onClick={(e) => { e.preventDefault(); resetState(); }}>try again</a>.
                        </p>
                      </>
                    )}
                  </div>
                  <p className="lead fw-normal text-white-50">
                    Profiles stored on your iPad can be exported using the
                    <br />
                    <Link href="/downloads">Custom Profile Manager</Link>.
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
