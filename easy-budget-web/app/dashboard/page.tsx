'use client';

import React from 'react';
import { PieChart, Pie, Cell, Tooltip, ResponsiveContainer } from 'recharts';
import RecentTransactions from '../dashboard/components/RecentTransactions';
import StatCard from '../dashboard/components/StatCard';

const data = [
  { name: 'Food & Drink', value: 750, color: '#8b5cf6' }, // Purple
  { name: 'Utilities', value: 250, color: '#f97316' }, // Orange
  { name: 'Subscriptions', value: 120, color: '#ef4444' }, // Red
  { name: 'Groceries', value: 380, color: '#10b981' }, // Emerald
  { name: 'Health/Other', value: 340, color: '#6b7280' }, // Gray
];

const DashboardPage: React.FC = () => {
  return (
    <div className="flex flex-col xl:flex-row gap-8 max-w-[1600px] mx-auto">
      {/* Left Column (Main Content) */}
      <div className="flex-1 space-y-8">
        
        {/* Controls */}
        <div className="flex flex-col sm:flex-row justify-between items-start sm:items-end gap-4">
          <div className="w-full sm:w-64">
             <label className="block text-sm font-semibold text-slate-600 mb-1">Active Account</label>
             <div className="relative">
               <select className="w-full appearance-none bg-white border border-slate-200 text-slate-900 font-bold py-3 pl-4 pr-10 rounded-xl focus:outline-none focus:ring-2 focus:ring-green-500 shadow-sm cursor-pointer">
                 <option>All Accounts</option>
                 <option>Checking (...8842)</option>
                 <option>Savings (...9221)</option>
               </select>
               <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-slate-500">
                 <span className="material-symbols-outlined">expand_more</span>
               </div>
             </div>
          </div>
          <div className="flex space-x-3">
            <button className="flex items-center space-x-2 bg-white border border-slate-200 text-slate-700 font-bold py-2.5 px-4 rounded-xl hover:bg-slate-50 transition shadow-sm">
               <span className="material-symbols-outlined text-[20px]">add_circle</span>
               <span>Add Transaction</span>
            </button>
            <button className="flex items-center space-x-2 bg-white border border-slate-200 text-slate-700 font-bold py-2.5 px-4 rounded-xl hover:bg-slate-50 transition shadow-sm">
               <span className="material-symbols-outlined text-[20px]">analytics</span>
               <span>Reports</span>
            </button>
          </div>
        </div>

        {/* Big Spending Card */}
        <div className="bg-white rounded-2xl p-6 border border-slate-100 shadow-sm">
           <div className="flex flex-col lg:flex-row gap-8">
              <div className="flex-1 flex flex-col justify-center">
                 <p className="text-slate-500 font-medium mb-1">Total Spent This Month</p>
                 <h2 className="text-4xl font-extrabold text-slate-900 mb-4">-$1,840.00</h2>
                 <div className="flex items-center gap-3 mb-6">
                    <span className="bg-red-100 text-red-700 text-xs font-bold px-2 py-1 rounded">-5.2%</span>
                    <span className="text-slate-400 text-sm">vs last month</span>
                 </div>
                 <div className="grid grid-cols-2 gap-4">
                    <div className="bg-slate-50 p-3 rounded-lg">
                       <p className="text-xs text-slate-500 mb-1">Income</p>
                       <p className="text-lg font-bold text-slate-800">+$4,200</p>
                    </div>
                    <div className="bg-slate-50 p-3 rounded-lg">
                       <p className="text-xs text-slate-500 mb-1">Spending</p>
                       <p className="text-lg font-bold text-slate-800">-$1,840</p>
                    </div>
                 </div>
              </div>

              {/* Chart */}
              <div className="flex-1 flex flex-col items-center">
                 <div className="relative h-64 w-64">
                    <ResponsiveContainer width="100%" height="100%">
                      <PieChart>
                        <Pie
                          data={data}
                          cx="50%"
                          cy="50%"
                          innerRadius={60}
                          outerRadius={80}
                          paddingAngle={5}
                          dataKey="value"
                        >
                          {data.map((entry, index) => (
                            <Cell key={`cell-${index}`} fill={entry.color} strokeWidth={0} />
                          ))}
                        </Pie>
                        <Tooltip 
                            contentStyle={{ borderRadius: '8px', border: 'none', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)' }}
                            formatter={(value) => [`$${value}`, 'Amount']}
                        />
                      </PieChart>
                    </ResponsiveContainer>
                    {/* Center Text Overlay */}
                    <div className="absolute inset-0 flex items-center justify-center pointer-events-none">
                       <div className="text-center">
                          <p className="text-xs font-bold text-slate-400 uppercase tracking-wide">Categories</p>
                       </div>
                    </div>
                 </div>
                 <div className="flex flex-wrap justify-center gap-3 mt-4">
                    {data.map((item) => (
                      <div key={item.name} className="flex items-center gap-1.5">
                         <div className="w-2.5 h-2.5 rounded-full" style={{ backgroundColor: item.color }}></div>
                         <span className="text-xs text-slate-500">{item.name}</span>
                      </div>
                    ))}
                 </div>
              </div>
           </div>
        </div>

        {/* Stats Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
           <StatCard 
              title="Top Spending Category" 
              icon="category" 
              iconColor="text-purple-500"
              mainValue="Food & Drink"
              subValue="$750 this month"
              actionText="View Breakdown"
              actionColor="text-green-600"
           />
           <StatCard 
              title="Average Weekly Spend" 
              icon="speed" 
              iconColor="text-orange-500"
              mainValue="$325.00/week"
              subValue="Current week vs. last"
              actionText="Review Trends"
              actionColor="text-green-600"
           />
           <StatCard 
              title="Recurring Bills Total" 
              icon="receipt_long" 
              iconColor="text-red-500"
              mainValue="-$1,560"
              subValue="7 bills this month"
              actionText="Manage Subscriptions"
              actionColor="text-green-600"
           />
            <StatCard 
              title="Savings Goal Progress" 
              icon="savings" 
              iconColor="text-blue-500"
              mainValue="72%"
              mainValueColor="text-blue-600"
              subValue="Towards Vacation Fund ($5k)"
              actionText="Update Goal"
              actionColor="text-green-600"
           />
           <StatCard 
              title="Net Income This Month" 
              icon="trending_up" 
              iconColor="text-green-500"
              mainValue="+$2,360"
              mainValueColor="text-green-600"
              subValue="After all income & expenses"
              actionText="View Cash Flow"
              actionColor="text-green-600"
           />
            <StatCard 
              title="Subscription Spending" 
              icon="autorenew" 
              iconColor="text-red-500"
              mainValue="-$120"
              mainValueColor="text-red-500"
              subValue="Across 5 active subs"
              actionText="Manage Subscriptions"
              actionColor="text-green-600"
           />
        </div>
      </div>

      {/* Right Column (Transactions) */}
      <div className="w-full xl:w-96 flex-shrink-0">
         <RecentTransactions />
      </div>
    </div>
  );
};

export default DashboardPage;