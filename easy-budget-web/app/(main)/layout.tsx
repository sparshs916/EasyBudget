import { redirect } from "next/navigation";
import { auth0 } from "@/lib/auth0";
import ClientLayout from "../ClientLayout";

export default async function MainLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  // Protect all routes in (main) group
  const session = await auth0.getSession();
  
  if (!session) {
    redirect("/auth/login");
  }

  return <ClientLayout>{children}</ClientLayout>;
}
