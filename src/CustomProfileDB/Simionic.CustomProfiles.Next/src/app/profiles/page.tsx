"use client";

import { Suspense } from "react";
import ProfileListContent from "./ProfileListContent";

export default function ProfileListPage() {
  return (
    <Suspense fallback={
      <section className="bg-white py-12 min-h-screen">
        <div className="text-center"><h5 className="text-black">Loading...</h5></div>
      </section>
    }>
      <ProfileListContent />
    </Suspense>
  );
}
