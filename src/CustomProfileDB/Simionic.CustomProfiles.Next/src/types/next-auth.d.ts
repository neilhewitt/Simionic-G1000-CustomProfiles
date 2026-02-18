import "next-auth";

declare module "next-auth" {
  interface Session {
    ownerId?: string;
  }
}

declare module "next-auth/jwt" {
  interface JWT {
    ownerId?: string;
  }
}
