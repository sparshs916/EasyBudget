'use client';

import React from 'react';
import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { useSidebar } from '../context/SidebarContext';

const navItems = [
  { name: 'Dashboard', path: '/dashboard', icon: 'dashboard' },
  { name: 'Budget', path: '/budget', icon: 'account_balance_wallet' },
  { name: 'Goals', path: '/goals', icon: 'track_changes' },
  { name: 'Connect', path: '/connect', icon: 'link' },
];

const Sidebar: React.FC = () => {
  const pathname = usePathname();
  const { isOpen } = useSidebar();

  return (
    <aside className={`
      h-full bg-white border-r border-slate-200 flex-shrink-0
      transition-all duration-300 ease-in-out overflow-hidden
      ${isOpen ? 'w-64' : 'w-0'}
    `}>
      <div className="flex flex-col h-full w-64">
        {/* Logo */}
        <div className="h-20 flex items-center px-8 border-b border-slate-100">
          <div className="w-8 h-8 bg-green-600 rounded-lg flex items-center justify-center mr-3">
            <span className="material-symbols-outlined text-white text-xl">payments</span>
          </div>
          <div>
            <h1 className="font-bold text-lg leading-tight">EasyBudget</h1>
            <p className="text-xs text-slate-400">Budgeting Made Easier</p>
          </div>
        </div>

        {/* Nav Links */}
        <nav className="flex-1 px-4 py-6 space-y-1 overflow-y-auto">
          <p className="px-4 text-xs font-semibold text-slate-400 uppercase tracking-wider mb-2">Main Menu</p>
          {navItems.map((item) => {
            const isActive = pathname === item.path || pathname?.startsWith(item.path + '/');
            return (
              <Link
                key={item.path}
                href={item.path}
                className={`
                  flex items-center px-4 py-3 text-sm font-medium rounded-xl transition-colors
                  ${isActive 
                    ? 'bg-green-50 text-green-700' 
                    : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900'}
                `}
              >
                <span className={`material-symbols-outlined mr-3 ${isActive ? 'fill-1' : ''}`}>
                  {item.icon}
                </span>
                {item.name}
              </Link>
            );
          })}

          <div className="mt-8">
            <p className="px-4 text-xs font-semibold text-slate-400 uppercase tracking-wider mb-2">Settings</p>
            <a href="#" className="flex items-center px-4 py-3 text-sm font-medium text-slate-500 rounded-xl hover:bg-slate-50 hover:text-slate-900 transition-colors">
              <span className="material-symbols-outlined mr-3">settings</span>
              Settings
            </a>
            <a href="#" className="flex items-center px-4 py-3 text-sm font-medium text-slate-500 rounded-xl hover:bg-slate-50 hover:text-slate-900 transition-colors">
              <span className="material-symbols-outlined mr-3">help</span>
              Help Center
            </a>
          </div>
        </nav>

        {/* User Profile */}
        <div className="p-4 border-t border-slate-100">
          <div className="flex items-center p-3 rounded-xl bg-slate-50 hover:bg-slate-100 cursor-pointer transition-colors">
            <div className="w-10 h-10 rounded-full bg-slate-200 flex items-center justify-center text-slate-500 mr-3">
              <span className="material-symbols-outlined">person</span>
            </div>
            <div>
              <p className="text-sm font-bold text-slate-700">User</p>
              <p className="text-xs text-slate-500">Pro Member</p>
            </div>
          </div>
        </div>
      </div>
    </aside>
  );
};

export default Sidebar;
