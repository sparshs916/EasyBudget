'use client';

import React from 'react';
import { usePathname } from 'next/navigation';
import { useSidebar } from '../context/SidebarContext';

const Header: React.FC = () => {
  const pathname = usePathname();
  const { isOpen, toggle } = useSidebar();

  const getPageTitle = () => {
    if (pathname?.startsWith('/goals')) return 'Financial Goals';
    if (pathname?.startsWith('/budget')) return 'Visual Budget';
    if (pathname?.startsWith('/connect')) return 'Connect Accounts';
    return 'Dashboard';
  };

  return (
    <header className="h-20 bg-white border-b border-slate-200 flex items-center justify-between px-6 lg:px-8">
      <div className="flex items-center gap-4">
        <button
          onClick={toggle}
          className="p-2 rounded-lg text-slate-500 hover:text-slate-800 hover:bg-slate-100 transition-colors"
          aria-label={isOpen ? 'Close sidebar' : 'Open sidebar'}
        >
          <span className="material-symbols-outlined">
            {isOpen ? 'menu_open' : 'menu'}
          </span>
        </button>
        <h2 className="text-xl font-bold text-slate-800 hidden sm:block">{getPageTitle()}</h2>
      </div>

      <div className="flex items-center space-x-4">
        <button className="flex items-center space-x-2 text-slate-500 hover:text-slate-800 px-3 py-2 rounded-lg hover:bg-slate-50 transition-colors">
          <span className="material-symbols-outlined">notifications</span>
          <span className="hidden sm:inline text-sm font-medium">Alerts</span>
        </button>
        <button  onClick={() => window.location.href = '/auth/logout'} 
        className="flex items-center space-x-2 bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded-lg transition-colors shadow-sm shadow-green-200">
          <span className="material-symbols-outlined text-[20px]">logout</span>
          <span className="hidden sm:inline text-sm font-bold">Logout</span>
        </button>
      </div>
    </header>
  );
};

export default Header;
