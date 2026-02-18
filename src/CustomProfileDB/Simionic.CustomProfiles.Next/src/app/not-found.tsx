import Link from "next/link";

export default function NotFound() {
  return (
    <main className="bg-dark py-5 flex-grow-1 d-flex align-items-center justify-content-center">
      <div className="text-center">
        <h1 className="display-5 fw-bolder text-white mb-4">404</h1>
        <h3 className="text-white-50 mb-4">Page not found</h3>
        <p className="text-white-50 mb-5">
          The page you&apos;re looking for doesn&apos;t exist or has been moved.
        </p>
        <Link href="/" className="btn btn-primary btn-lg px-4">
          Go home
        </Link>
      </div>
    </main>
  );
}
