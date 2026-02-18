"use client";

import { useState } from "react";

interface FAQEntry {
  question: string;
  answer: string[];
}

const faqEntries: FAQEntry[] = [
  {
    question: "How do I use these profiles with my Simionic apps?",
    answer: [
      "To use the profiles stored here with your apps, you need to first make sure you're on the latest version of the app. Some older iPads cannot run the latest versions and may not be able to use custom profile data. Once you have this, touch the spot at the top-right of the PFD / MFD screen to access the settings. You should see a tab marked 'Aircraft' - select this. At the top should be an option 'Customisation'. Enabling this requires an in-app purchase.",
      "Once you have paid for the feature and it's available, you can touch 'Customisation' and this will open a dialog box where you can choose the settings for your custom aircraft profile. The profile page on this site is layed out similarly to the custom profile dialog in the app. You can transpose the options and data from the profile screen here into the dialog in your app. You can also import profiles using the Custom Profile Manager tool.",
    ],
  },
  {
    question: "Do I have to log in to use this site?",
    answer: [
      "No, not to search and use profiles.",
      'However, if you want to <a href="/create">create a profile</a>, or edit a profile you have created previously, then you do need to log in using a personal Microsoft Account. To do this, click on the \'Log In\' link in the navigation bar, or when prompted to do so. Once logged in you will see an edit button appear on your profiles, and you will have access to create profiles.',
    ],
  },
  {
    question: "How do I share a profile with the community?",
    answer: [
      "There are two ways to share a profile.",
      'The simplest is to install the <a href="/downloads">Simionic Custom Profile Manager</a> desktop app and use this to export your profile to a JSON file, which you can then <a href="/import">import</a> into the site. The profile will be immediately saved to the database as a draft that you can publish when you\'re ready.',
      'Alternatively, you can <a href="/create">create</a> a new empty profile. Give your profile a name (you can change this later), and click the \'create\' button.',
      "This will open the profile editing screen. Fill out the details, and when you're ready, you can click 'save draft' at the bottom of the form. This will save your work to the database, but in a draft (unpublished) state so that only you can see it. When you're happy with your new profile, you can set the status to 'published' using the button at the top of the form, and then save again. This will publish your profile for everyone to see.",
      "If you want to edit a profile you created, once logged in, you will see 'edit' buttons on every profile you have contributed on the browse profiles screen. You can filter the view to show just your profiles, or just your profiles in draft. Click 'edit' to open the edit view, make your changes, and hit 'save' again to save changes. Once saved, previous changes are lost. You can also un-publish a published profile, but for the sake of the community we would ask that you don't do that without a good reason.",
    ],
  },
  {
    question: "Does the site store my personal data, such as passwords?",
    answer: [
      "The site does not store any passwords or sensitive data. All login functionality is provided via Microsoft's OpenID Connect service. The site never sees your password or personal information, except for your name (as specified in your Microsoft Account profile) and the email address that you use to log in to your Microsoft Account.",
      "Your name is stored in the database on any profiles that you create. Your email address is not stored, instead a unique identifier is generated which is used to link the profiles you create to your Microsoft Account.",
    ],
  },
  {
    question: "Why do I have to use a Microsoft Account? Why a personal one?",
    answer: [
      "Microsoft Accounts are a commonly-used ID standard, alongside Google ID, Apple ID and various other providers. While we may extend the login system to support Google and other OpenID logins in the future, the Microsoft Account was the easiest provider to integrate and that's why we used it. The reason it needs to be a personal account is that using an account tied to a work or business email address means you would lose access to your profiles if you were to - for example - change jobs and lose access to that account.",
      'If you want to use the site and don\'t have a Microsoft ID, you can easily create one <a href="https://account.microsoft.com/account" target="_blank" rel="noopener noreferrer">here</a>. You can create one for any email address you may use including Gmail, Apple etc.',
    ],
  },
  {
    question: "How do I contact the site administrator?",
    answer: [
      'If you need to contact us, please use the email on our <a href="/contact">Contact Us</a> page. We will try to reply as quickly as possible, but remember that this site is run by volunteers purely as a hobby project.',
    ],
  },
  {
    question: "Can I donate to help with the running costs?",
    answer: [
      "It's nice of you to ask, but we aren't looking for donations at the moment! If running the site becomes expensive and the site is popular, we may set up a donation link.",
    ],
  },
];

function FAQItem({ entry, index }: { entry: FAQEntry; index: number }) {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <>
      <h5 className="text-white mt-2">
        <a
          className="text-decoration-none text-white"
          href={`#faq${index}`}
          role="button"
          aria-expanded={isOpen}
          onClick={(e) => {
            e.preventDefault();
            setIsOpen(!isOpen);
          }}
        >
          {entry.question}
        </a>
      </h5>
      {isOpen && (
        <div>
          {entry.answer.map((para, i) => (
            <p
              key={i}
              className="text-white"
              dangerouslySetInnerHTML={{ __html: para }}
            />
          ))}
        </div>
      )}
    </>
  );
}

export default function FAQPage() {
  return (
    <main className="bg-dark py-5">
      <div className="container px-5">
        <div className="row gx-5 align-items-center justify-content-center">
          <div className="col-10">
            <div className="my-5 text-body fw-normal">
              {/* eslint-disable-next-line @next/next/no-img-element */}
              <img className="img-fluid rounded-3 ml-5 mb-5 float-end" src="/img/G1000_screen.jpg" width={300} alt="G1000 MFD & PFD set into a cockpit instrument panel" />
              <h3 className="fw-bolder text-white mb-5">Frequently Asked Questions</h3>
              {faqEntries.map((entry, i) => (
                <FAQItem key={i} entry={entry} index={i + 1} />
              ))}
            </div>
          </div>
        </div>
      </div>
    </main>
  );
}
