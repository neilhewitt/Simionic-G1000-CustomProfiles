import { promises as fs } from "fs";
import path from "path";
import { Profile, ProfileSummary } from "@/types";
import { fixUpGauges } from "./profile-utils";

const DATA_DIR = path.join(process.cwd(), "data");

async function ensureDataDir(): Promise<void> {
  try {
    await fs.access(DATA_DIR);
  } catch {
    await fs.mkdir(DATA_DIR, { recursive: true });
  }
}

function profilePath(id: string): string {
  // Sanitise: only allow hex chars and hyphens (GUID format)
  const safe = id.replace(/[^a-zA-Z0-9\-]/g, "");
  return path.join(DATA_DIR, `${safe}.json`);
}

export async function getAllProfiles(): Promise<ProfileSummary[]> {
  await ensureDataDir();

  const files = await fs.readdir(DATA_DIR);
  const jsonFiles = files.filter((f) => f.endsWith(".json"));

  const summaries: ProfileSummary[] = [];

  for (const file of jsonFiles) {
    try {
      const raw = await fs.readFile(path.join(DATA_DIR, file), "utf-8");
      const profile: Profile = JSON.parse(raw);
      summaries.push({
        id: profile.id!,
        Owner: profile.Owner,
        LastUpdated: profile.LastUpdated,
        Name: profile.Name,
        AircraftType: profile.AircraftType,
        Engines: profile.Engines,
        IsPublished: profile.IsPublished,
        Notes: profile.Notes,
      });
    } catch {
      // Skip malformed files
    }
  }

  return summaries;
}

export async function getProfile(id: string): Promise<Profile | null> {
  try {
    const raw = await fs.readFile(profilePath(id), "utf-8");
    const profile: Profile = JSON.parse(raw);
    fixUpGauges(profile);
    return profile;
  } catch {
    return null;
  }
}

export async function upsertProfile(id: string, profile: Profile): Promise<void> {
  await ensureDataDir();

  profile.id = id;
  profile.LastUpdated = new Date().toISOString();

  await fs.writeFile(profilePath(id), JSON.stringify(profile, null, 2), "utf-8");
}
