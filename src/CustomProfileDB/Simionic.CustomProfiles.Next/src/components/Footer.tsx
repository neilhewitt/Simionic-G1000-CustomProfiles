import Link from "next/link";

export default function Footer() {
  return (
    <footer className="bg-dark py-4 mt-auto">
      <div className="container px-5">
        <div className="row align-items-center justify-content-center flex-column flex-sm-row">
          <div className="col-auto">
            <Link href="/downloads" className="link-light small">Downloads</Link>
            <span className="text-white mx-1">|</span>
            <Link href="/about" className="link-light small">About</Link>
            <span className="text-white mx-1">|</span>
            <Link href="/faq" className="link-light small">FAQ</Link>
            <span className="text-white mx-1">|</span>
            <Link href="/contact" className="link-light small">Contact</Link>
            <span className="text-white mx-1">|</span>
            <Link href="/privacy" className="link-light small">Privacy</Link>
            <span className="text-white mx-1">|</span>
            <Link href="/terms" className="link-light small">T&amp;C</Link>
          </div>
        </div>
      </div>
    </footer>
  );
}
