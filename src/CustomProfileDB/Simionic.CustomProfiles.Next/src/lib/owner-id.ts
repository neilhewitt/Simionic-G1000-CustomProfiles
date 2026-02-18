import { pbkdf2Sync } from "crypto";

// Must match the C# Helper.cs implementation exactly:
// Salt: base64-decoded "AWBH+yXC3ba1vxMj3MrnuXKHikL2RDSX"
// Iterations: 100,000
// Key length: 24 bytes
// Hash: SHA-1 (Rfc2898DeriveBytes default)
// Output: uppercase hex with no separators

const CRYPTO_SALT = Buffer.from("AWBH+yXC3ba1vxMj3MrnuXKHikL2RDSX", "base64");
const CRYPTO_ITERATIONS = 100000;
const CRYPTO_BYTES = 24;

export function getOwnerId(email: string): string {
  const derived = pbkdf2Sync(email, CRYPTO_SALT, CRYPTO_ITERATIONS, CRYPTO_BYTES, "sha1");
  return derived.toString("hex").toUpperCase();
}
