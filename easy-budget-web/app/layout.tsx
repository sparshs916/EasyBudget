import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import "./globals.css"
import { Auth0Provider } from "@auth0/nextjs-auth0/client";


const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "EasyBudget - Financial Command Center",
  description: "Track your finances, set goals, and build your financial future",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <head>
        <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:opsz,wght,FILL,GRAD@20..48,100..700,0..1,-50..200&display=optional" />
      </head>
      <body
        className={`${geistSans.variable} ${geistMono.variable} antialiased`}
      > 
              <Auth0Provider>

        {children}
                </Auth0Provider>
      </body>
    </html>
  );
}
