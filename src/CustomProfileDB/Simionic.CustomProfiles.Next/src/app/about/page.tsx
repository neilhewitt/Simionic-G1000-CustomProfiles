import Link from "next/link";

export default function AboutPage() {
  return (
    <main className="bg-dark py-5">
      <div className="container px-5">
        <div className="row gx-5 align-items-center justify-content-center">
          <div className="col-8">
            <div className="my-5 text-body fw-normal">
              {/* eslint-disable-next-line @next/next/no-img-element */}
              <img className="img-fluid rounded-3 ml-5 mb-5 float-end" src="/img/G1000_screen.jpg" width={300} alt="G1000 MFD & PFD set into a cockpit instrument panel" />
              <h2 className="fw-bolder text-white mb-2">About this site</h2>
              <p className="text-white">
                This site is a place to find and share custom profiles for the{" "}
                <a href="https://www.simionic.net/wordpress/g1000-apps/">Simionic G1000 apps</a>. These apps are simulations of the Garmin G1000 Primary Flight Display and Multi-Function Display avionics units
                which run on iPads and can be connected to flight simulation software including Microsoft Flight Simulator, X-Plane and Prepar3d.
              </p>
              <p className="text-white">
                The apps support the configuration of several aircraft out of the box,
                but they also allow you to create your own configurations which will set up the engine indicators in the app to match the style and calibration of those for the G1000 versions of
                other aircraft.
              </p>
              <p className="text-white">
                These profiles are stored locally on your iPad, and to share them with other users, you need to export the data to a common format. The{" "}
                <Link href="/downloads">Custom Profile Manager</Link> makes this possible.
                This site is intended to be a one-stop shop where you can find the data from profiles that users have created.
              </p>
              <p className="text-white">
                Anyone can <Link href="/profiles">browse the profiles</Link> that are stored in the database here, and when you find the one you&apos;re looking for, you can click through and see a representation of the custom profile dialog
                from the iPad app with the data for that profile. While Simionic does not provide a way to export or import profiles, you can now do so with our{" "}
                <Link href="/downloads">Custom Profile Manager</Link>. Profiles that you download
                from this site can be imported into your iPad using the tool.
              </p>
              <p className="text-white">
                If you have existing profiles and you would like to contribute to the community, you can{" "}
                <Link href="/create">create</Link> or{" "}
                <Link href="/import">import</Link> a profile.
              </p>
            </div>
          </div>
        </div>
      </div>
    </main>
  );
}
