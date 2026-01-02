'use client';

import { SidebarProvider } from './context/SidebarContext';
import Sidebar from './components/Sidebar';
import Header from './components/Header';

export default function ClientLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <SidebarProvider>
      <div className="flex h-screen bg-[#f8fafc] text-slate-800 overflow-hidden">
        <Sidebar />
        <main className="flex-1 flex flex-col min-w-0 overflow-hidden transition-all duration-300 ease-in-out">
          <Header />
          <div className="flex-1 overflow-auto p-4 lg:p-8">
            {children}
          </div>
        </main>
      </div>
    </SidebarProvider>
  );
}
