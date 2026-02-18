import type { Metadata } from "next";
import "./globals.css";
import Navbar from "@/components/Navbar";
import Footer from "@/components/Footer";
import AuthProvider from "@/components/AuthProvider";

export const metadata: Metadata = {
  title: "G1000 Profile DB",
  description: "Search and find custom aircraft profiles for the Simionic G1000 apps, contributed by the community.",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <head>
        <link href="/css/open-iconic/font/css/open-iconic-bootstrap.min.css" rel="stylesheet" />
        <link href="/css/start-bootstrap-5.0.5-custom.css" rel="stylesheet" />
        <link href="/css/g1000-profile-db.css" rel="stylesheet" />
        <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/bootstrap-icons.css" rel="stylesheet" />
      </head>
      <body className="bg-dark d-flex flex-column" style={{ minHeight: "100vh" }}>
        <AuthProvider>
          <Navbar />
          {children}
          <Footer />
        </AuthProvider>
      </body>
    </html>
  );
}
