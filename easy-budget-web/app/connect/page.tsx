"use client"; // Required for browser-only hooks like useEffect

import { useEffect, useState } from "react";
import Script from "next/script";

declare global {
  interface Window {
    TellerConnect: any;
  }
}

interface TellerEnrollment {
  accessToken: string;
  user: {
    id: string;
  };
  enrollment: {
    id: string;
    institution: {
      name: string;
    };
  };
  signatures: string[];
}

export default function Connect() {
  const [teller, setTeller] = useState<any>(null);
  const [isLoaded, setIsLoaded] = useState(false);
  const [nonce, setNonce] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [isConnecting, setIsConnecting] = useState(false);
  
  const accessToken = process.env.NEXT_PUBLIC_AUTH0_TEST_TOKEN;
  // Fetch nonce from backend before initializing Teller
  const fetchNonce = async (): Promise<string | null> => {
    try {
      const res = await fetch(
        `${process.env.NEXT_PUBLIC_API_URL}/api/enrollment/nonce`,
        {
          credentials: "include",
          headers: {
            "Content-Type": "application/json",
            // Add your Auth0 token here
            Authorization: `Bearer ${accessToken}`,
          },
        }
      );

      if (!res.ok) {
        throw new Error("Failed to fetch nonce");
      }

      const data = await res.json();
      setNonce(data.nonce);
      return data.nonce;
    } catch (err) {
      console.error("Error fetching nonce:", err);
      setError("Failed to initialize secure connection. Please try again.");
      return null;
    }
  };

  const initializeTeller = async () => {
    if (!window.TellerConnect) return;

    const appId = process.env.NEXT_PUBLIC_TELLER_APP_ID;
    if (!appId) {
      console.error(
        "Teller Application ID missing: set NEXT_PUBLIC_TELLER_APP_ID in .env.local and restart dev server"
      );
      return;
    }

    // Fetch nonce from backend
    const currentNonce = await fetchNonce();
    if (!currentNonce) return;

    const setup = window.TellerConnect.setup({
      applicationId: appId,
      products: ["verify", "transactions", "balance"],
      environment: "sandbox",
      nonce: currentNonce, // Pass nonce for signature verification
      onInit: () => console.log("Teller initialized"),
      onSuccess: async (enrollment: TellerEnrollment) => {
        console.log("Enrollment Success!", enrollment);
        setIsConnecting(true);
        setError(null);

        try {
          // Send enrollment to backend with signatures for verification
          const res = await fetch(
            `${process.env.NEXT_PUBLIC_API_URL}/api/enrollment`,
            {
              method: "POST",
              credentials: "include",
              headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
              },
              body: JSON.stringify({
                accessToken: enrollment.accessToken,
                userId: enrollment.user.id,
                enrollmentId: enrollment.enrollment.id,
                institutionId: "", // Teller doesn't provide this in the enrollment object
                institutionName: enrollment.enrollment.institution.name,
                signatures: enrollment.signatures,
                environment: "sandbox",
              }),
            }
          );

          if (!res.ok) {
            const errorData = await res.json();
            throw new Error(errorData.message || "Failed to create enrollment");
          }

          const data = await res.json();
          console.log("Enrollment created:", data);
          // Redirect to dashboard or next step
          // window.location.href = "/dashboard";
        } catch (err: any) {
          console.error("Error creating enrollment:", err);
          setError(
            err.message || "Failed to connect bank account. Please try again."
          );
        } finally {
          setIsConnecting(false);
        }
      },
      onExit: () => {
        console.log("User closed Teller Connect");
        // Refresh nonce for next attempt
        fetchNonce();
      },
    });

    setTeller(setup);
    setIsLoaded(true);
  };

  const handleConnect = async () => {
    if (!teller) return;

    // Refresh nonce before opening (in case previous one expired)
    const freshNonce = await fetchNonce();
    if (freshNonce) {
      teller.open();
    }
  };

  return (
    <main className="flex min-h-screen flex-col items-center justify-center p-24 bg-gray-50">
      {/* 1. Load the Teller Client Library */}
      <Script
        src="https://cdn.teller.io/connect/connect.js"
        onLoad={initializeTeller}
      />

      <div className="bg-white p-8 rounded-xl shadow-lg text-center max-w-md">
        <h1 className="text-3xl font-bold text-gray-900 mb-2">
          Sync Your Bank
        </h1>
        <p className="text-gray-600 mb-8">
          Securely connect your accounts using Teller to start tracking your
          budget.
        </p>

        {error && (
          <div className="mb-4 p-3 bg-red-100 text-red-700 rounded-lg text-sm">
            {error}
          </div>
        )}

        {/* 2. The Interaction Button */}
        <button
          onClick={handleConnect}
          disabled={!isLoaded || isConnecting}
          className={`px-8 py-4 rounded-full font-bold text-white transition-all 
            ${
              isLoaded && !isConnecting
                ? "bg-blue-600 hover:bg-blue-700 shadow-md active:scale-95"
                : "bg-gray-400 cursor-not-allowed"
            }`}
        >
          {isConnecting
            ? "Connecting..."
            : isLoaded
            ? "Connect Bank Account"
            : "Loading Teller..."}
        </button>
      </div>
    </main>
  );
}
