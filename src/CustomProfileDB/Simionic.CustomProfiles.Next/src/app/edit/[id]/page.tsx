"use client";

import { Suspense } from "react";
import EditProfileContent from "./EditProfileContent";

export default function EditProfilePage() {
  return (
    <Suspense fallback={
      <section className="bg-white py-12 min-h-screen">
        <div className="text-center"><h5 className="text-black">Loading...</h5></div>
      </section>
    }>
      <EditProfileContent />
    </Suspense>
  );
}
