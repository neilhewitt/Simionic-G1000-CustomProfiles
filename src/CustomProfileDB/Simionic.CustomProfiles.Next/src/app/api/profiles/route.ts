import { NextResponse } from "next/server";
import { getAllProfiles } from "@/lib/data-store";

export async function GET() {
  try {
    const profiles = await getAllProfiles();
    return NextResponse.json(profiles);
  } catch (error) {
    console.error("Failed to fetch profiles:", error);
    return NextResponse.json({ error: "Failed to fetch profiles" }, { status: 500 });
  }
}
