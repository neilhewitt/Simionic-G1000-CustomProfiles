import { NextAuthOptions } from "next-auth";
import AzureADProvider from "next-auth/providers/azure-ad";
import { getOwnerId } from "./owner-id";

export const authOptions: NextAuthOptions = {
  providers: [
    AzureADProvider({
      clientId: process.env.AZURE_AD_CLIENT_ID!,
      clientSecret: process.env.AZURE_AD_CLIENT_SECRET!,
      tenantId: "consumers",
      authorization: {
        params: {
          scope: "openid email profile",
        },
      },
    }),
  ],
  callbacks: {
    async jwt({ token, account }) {
      if (account && token.email) {
        // Compute ownerId locally using the same PBKDF2 algorithm as the C# backend
        token.ownerId = getOwnerId(token.email);
      }
      return token;
    },
    async session({ session, token }) {
      if (token.ownerId) {
        (session as any).ownerId = token.ownerId;
      }
      return session;
    },
  },
  pages: {
    signIn: "/auth/signin",
  },
};
