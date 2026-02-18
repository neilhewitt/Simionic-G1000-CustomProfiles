import { NextRequest, NextResponse } from "next/server";
import { getProfile, upsertProfile } from "@/lib/data-store";
import { getServerSession } from "next-auth";
import { authOptions } from "@/lib/auth";
import { Profile } from "@/types";

export async function GET(
  _request: NextRequest,
  { params }: { params: Promise<{ id: string }> }
) {
  try {
    const { id } = await params;
    const profile = await getProfile(id);
    if (!profile) {
      return NextResponse.json({ error: "Profile not found" }, { status: 404 });
    }
    return NextResponse.json(profile);
  } catch (error) {
    console.error("Failed to fetch profile:", error);
    return NextResponse.json({ error: "Failed to fetch profile" }, { status: 500 });
  }
}

export async function POST(
  request: NextRequest,
  { params }: { params: Promise<{ id: string }> }
) {
  try {
    const session = await getServerSession(authOptions);
    if (!session?.user) {
      return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
    }

    const { id } = await params;
    const profile: Profile = await request.json();

    // Inject owner info from session
    profile.Owner = {
      Id: (session as any).ownerId ?? null,
      Name: session.user.name ?? null,
    };

    await upsertProfile(id, profile);
    return NextResponse.json({ success: true });
  } catch (error) {
    console.error("Failed to upsert profile:", error);
    return NextResponse.json({ error: "Failed to save profile" }, { status: 500 });
  }
}
