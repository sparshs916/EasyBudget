"use client";

import React from 'react';
import { PieChart, Pie, Cell, ResponsiveContainer } from 'recharts';
import BudgetSection from '../budget/components/BudgetSection';

const incomeData = [
  { name: 'Fixed', value: 2000, color: '#ef4444' }, // Red
  { name: 'Variable', value: 1500, color: '#f97316' }, // Orange
  { name: 'Goals', value: 1000, color: '#3b82f6' }, // Blue
  { name: 'Unallocated', value: 500, color: '#e5e7eb' }, // Gray
];

const BudgetPage: React.FC = () => {
  return (
    <div className="max-w-[1400px] mx-auto space-y-8">
      
      {/* Top Section */}
      <div className="flex flex-col md:flex-row gap-6 justify-between items-start md:items-center">
         <div>
            <h1 className="text-3xl font-extrabold text-slate-900 tracking-tight">Your Monthly Budget</h1>
            <p className="text-lg text-slate-500 mt-1">Allocate your income and achieve your financial goals.</p>
         </div>
         <div className="flex gap-3">
             <button className="flex items-center gap-2 px-6 py-3 rounded-xl bg-white border border-slate-200 text-green-600 font-bold hover:bg-green-50 transition-colors shadow-sm">
                <span className="material-symbols-outlined">add</span>
                Add Expense
             </button>
             <button className="flex items-center gap-2 px-6 py-3 rounded-xl bg-green-500 text-white font-bold hover:bg-green-600 transition-colors shadow-md shadow-green-200">
                <span className="material-symbols-outlined">add_circle</span>
                Set New Goal
             </button>
         </div>
      </div>

      {/* Income Distribution Card */}
      <div className="bg-white rounded-2xl p-6 lg:p-8 border border-slate-200 shadow-sm">
         <div className="flex flex-col lg:flex-row items-center gap-8 lg:gap-16">
            
            {/* Chart Area */}
            <div className="relative w-64 h-64 flex-shrink-0">
               <ResponsiveContainer width="100%" height="100%">
                  <PieChart>
                     <Pie
                        data={incomeData}
                        cx="50%"
                        cy="50%"
                        innerRadius={85}
                        outerRadius={110}
                        paddingAngle={2}
                        dataKey="value"
                        startAngle={90}
                        endAngle={-270}
                     >
                        {incomeData.map((entry, index) => (
                           <Cell key={`cell-${index}`} fill={entry.color} strokeWidth={0} />
                        ))}
                     </Pie>
                  </PieChart>
               </ResponsiveContainer>
               {/* Center Content */}
               <div className="absolute inset-0 flex flex-col items-center justify-center pointer-events-none">
                  <p className="text-xs font-bold text-slate-400 uppercase tracking-wider mb-1">Unallocated</p>
                  <p className="text-3xl font-extrabold text-slate-900">$500</p>
                  <button className="pointer-events-auto mt-2 flex items-center gap-1 px-3 py-1 bg-green-500 text-white rounded-full text-xs font-bold hover:bg-green-600 transition-colors">
                     <span className="material-symbols-outlined text-[14px]">add_task</span>
                     Allocate
                  </button>
               </div>
            </div>

            {/* Legend / Breakdown */}
            <div className="flex-1 w-full">
               <h3 className="text-xl font-bold text-slate-900 mb-2">Income Distribution</h3>
               <p className="text-slate-500 text-sm mb-6">Total Salary: <strong className="text-slate-900">$5,000</strong>. Allocated: <strong className="text-slate-900">$4,500</strong>.</p>
               
               <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
                  <div className="bg-slate-50 p-4 rounded-xl text-center border-b-4 border-red-500">
                     <p className="text-xs font-bold text-slate-400 uppercase mb-1">Fixed</p>
                     <p className="text-xl font-extrabold text-red-500">$2,000</p>
                  </div>
                  <div className="bg-slate-50 p-4 rounded-xl text-center border-b-4 border-orange-500">
                     <p className="text-xs font-bold text-slate-400 uppercase mb-1">Variable</p>
                     <p className="text-xl font-extrabold text-orange-500">$1,500</p>
                  </div>
                  <div className="bg-slate-50 p-4 rounded-xl text-center border-b-4 border-blue-500">
                     <p className="text-xs font-bold text-slate-400 uppercase mb-1">Goals</p>
                     <p className="text-xl font-extrabold text-blue-500">$1,000</p>
                  </div>
                  <div className="bg-slate-50 p-4 rounded-xl text-center border-b-4 border-slate-300">
                     <p className="text-xs font-bold text-slate-400 uppercase mb-1">Leftover</p>
                     <p className="text-xl font-extrabold text-slate-500">$500</p>
                  </div>
               </div>
            </div>
         </div>
      </div>

      {/* Budget Categories Grid */}
      <div className="space-y-4">
         <div className="flex items-center justify-between">
            <h3 className="text-xl font-bold text-slate-900">Budget Categories</h3>
            <button className="px-3 py-1.5 bg-slate-200 text-slate-600 text-xs font-bold rounded-lg hover:bg-slate-300 transition-colors">Add Category</button>
         </div>
         
         <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <BudgetSection 
               title="Fixed Expenses"
               subtitle="Pending items are active."
               icon="auto_stories"
               iconColor="text-red-500"
               bgColor="bg-red-50"
               totalLabel="Total Fixed"
               totalValue="$2,000"
               totalColor="text-red-500"
               manageText="Manage Fixed Expenses"
               items={[
                  { label: 'Rent/Mortgage', amount: '$1,200', status: 'paid' },
                  { label: 'Utilities', amount: '$250', status: 'pending', accent: true },
                  { label: 'Car Payment', amount: '$300', status: 'paid' },
                  { label: 'Insurance', amount: '$250', status: 'pending', accent: true },
               ]}
            />
            
            <BudgetSection 
               title="Variable Expenses"
               subtitle="Real-time spending tracking."
               icon="shopping_bag"
               iconColor="text-orange-500"
               bgColor="bg-orange-50"
               totalLabel="Total Variable"
               totalValue="$1,500"
               totalColor="text-orange-500"
               manageText="Manage Variable Expenses"
               progressItems={[
                  { label: 'Groceries', current: 380, max: 500, color: 'bg-orange-500' },
                  { label: 'Dining Out', current: 280, max: 300, color: 'bg-orange-500', alert: 'Approaching Limit' },
                  { label: 'Entertainment', current: 100, max: 250, color: 'bg-orange-500' },
                  { label: 'Shopping', current: 150, max: 450, color: 'bg-orange-500' },
               ]}
            />

            <BudgetSection 
               title="Goal Contributions"
               subtitle="Monthly allocation progress."
               icon="military_tech"
               iconColor="text-blue-500"
               bgColor="bg-blue-50"
               totalLabel="Total Goals"
               totalValue="$1,000"
               totalColor="text-blue-500"
               manageText="Manage Goals"
               progressItems={[
                  { label: 'Emergency Fund', current: 300, max: 300, color: 'bg-blue-500', badge: 'Completed', subtext: 'Goal: $300' },
                  { label: 'House Deposit', current: 250, max: 500, color: 'bg-blue-500', subtext: 'Goal: $500' },
                  { label: 'New Car', current: 0, max: 200, color: 'bg-blue-500', subtext: 'Goal: $200' },
               ]}
            />
         </div>
      </div>
    </div>
  );
};

export default BudgetPage;