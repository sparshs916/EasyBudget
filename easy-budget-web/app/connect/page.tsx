"use client"; // Required for browser-only hooks like useEffect

import { useEffect, useState } from "react";
import Script from "next/script";

declare global {
  interface Window {
    TellerConnect: any;
  }
}

export default function Connect() {
  const [teller, setTeller] = useState<any>(null);
  const [isLoaded, setIsLoaded] = useState(false);

  const initializeTeller = () => {
    if (window.TellerConnect) {
      const appId = process.env.NEXT_PUBLIC_TELLER_APP_ID;

      if (!appId) {
        console.error(
          "Teller Application ID missing: set NEXT_PUBLIC_TELLER_APP_ID in .env.local and restart dev server"
        );
        return;
      }
      const setup = window.TellerConnect.setup({
        applicationId: appId,
        products: ["verify", "transactions", "balance"],
        environment : "sandbox",
        onInit: () => console.log("Teller initialized"),
        onSuccess: (enrollment: any) => {
          console.log("Enrollment Success!", enrollment.accessToken);
        },
        onExit: () => console.log("User closed Teller Connect"),
      });
      setTeller(setup);
      setIsLoaded(true);
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

        {/* 2. The Interaction Button */}
        <button
          onClick={() => teller?.open()}
          disabled={!isLoaded}
          className={`px-8 py-4 rounded-full font-bold text-white transition-all 
            ${
              isLoaded
                ? "bg-blue-600 hover:bg-blue-700 shadow-md active:scale-95"
                : "bg-gray-400 cursor-not-allowed"
            }`}
        >
          {isLoaded ? "Connect Bank Account" : "Loading Teller..."}
        </button>
      </div>
    </main>
  );
}
