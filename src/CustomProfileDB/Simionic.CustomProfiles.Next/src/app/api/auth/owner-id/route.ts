import { NextRequest, NextResponse } from "next/server";
import { getOwnerId } from "@/lib/owner-id";

export async function GET(request: NextRequest) {
  const email = request.nextUrl.searchParams.get("email");
  if (!email) {
    return NextResponse.json({ error: "email parameter required" }, { status: 400 });
  }

  try {
    const ownerId = getOwnerId(email);
    return NextResponse.json({ ownerId });
  } catch (error) {
    console.error("Failed to get owner ID:", error);
    return NextResponse.json({ error: "Failed to get owner ID" }, { status: 500 });
  }
}
