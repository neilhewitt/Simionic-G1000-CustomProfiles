export default function ContactPage() {
  return (
    <main className="bg-dark py-5">
      <div className="container px-5">
        <div className="row gx-5 align-items-center justify-content-center">
          <div className="col-8">
            <div className="my-5 text-body fw-normal text-white">
              {/* eslint-disable-next-line @next/next/no-img-element */}
              <img className="img-fluid rounded-3 ml-5 mb-5 float-end" src="/img/G1000_screen.jpg" width={300} alt="G1000 MFD & PFD set into a cockpit instrument panel" />
              <h2 className="fw-bolder mb-2 text-white">Contact us</h2>
              <p className="text-white">
                You can contact us via email at{" "}
                <a href="mailto:admin@g1000profiledb.com">admin@g1000profiledb.com</a>
              </p>
            </div>
          </div>
        </div>
      </div>
    </main>
  );
}
