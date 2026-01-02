import React from 'react';
import GoalCard from '../goals/components/GoalCard';
import ContributionHistory from '../goals/components/ContributionHistory';
import { Goal } from '../types';

const activeGoals: Goal[] = [
  { id: '1', title: 'House Downpayment', targetDate: 'Dec 2025', currentAmount: 45000, targetAmount: 100000, icon: 'home', colorClass: 'bg-orange-50 text-orange-500' },
  { id: '2', title: 'Japan Trip', targetDate: 'Apr 2024', currentAmount: 4200, targetAmount: 5000, icon: 'flight', colorClass: 'bg-blue-50 text-blue-500' },
  { id: '3', title: 'Emergency Fund', targetDate: 'ASAP', currentAmount: 8500, targetAmount: 15000, icon: 'medical_services', colorClass: 'bg-red-50 text-red-500' },
  { id: '4', title: 'New MacBook', targetDate: 'Nov 2023', currentAmount: 1800, targetAmount: 2500, icon: 'laptop_mac', colorClass: 'bg-purple-50 text-purple-500' },
  { id: '5', title: 'Car Upgrade', targetDate: '2026', currentAmount: 5000, targetAmount: 30000, icon: 'directions_car', colorClass: 'bg-yellow-50 text-yellow-600' },
];

const GoalsPage: React.FC = () => {
  return (
    <div className="max-w-[1600px] mx-auto">
      {/* Header Area */}
      <div className="flex flex-col md:flex-row justify-between items-start md:items-center mb-8 gap-4">
         <div>
            <h1 className="text-3xl font-extrabold text-slate-900 tracking-tight">Financial Goals</h1>
            <p className="text-slate-500 mt-1">Track your savings targets and build your future.</p>
         </div>
         <button className="flex items-center justify-center gap-2 rounded-xl h-12 px-6 bg-green-500 hover:bg-green-600 text-white text-sm font-bold tracking-wide transition-all shadow-lg shadow-green-200 active:scale-95">
            <span className="material-symbols-outlined text-[20px]">add_circle</span>
            <span>Create New Goal</span>
         </button>
      </div>

      {/* Progress Hero */}
      <div className="bg-white rounded-2xl p-6 lg:p-8 border border-slate-200 shadow-sm mb-10">
         <div className="flex justify-between items-start mb-4">
            <p className="text-xs font-bold text-slate-400 uppercase tracking-wider">Overall Savings Progress</p>
            <span className="material-symbols-outlined text-green-500">pie_chart</span>
         </div>
         <div className="flex flex-col sm:flex-row sm:items-end justify-between gap-4 mb-4">
            <h2 className="text-5xl font-extrabold text-slate-900">64%</h2>
            <div className="text-right">
               <p className="text-sm text-slate-400 mb-1">Total accumulated</p>
               <p className="text-xl font-bold text-slate-900">$65,000 <span className="text-slate-400">/ $101.5k</span></p>
            </div>
         </div>
         <div className="w-full h-3 bg-slate-100 rounded-full overflow-hidden">
            <div className="h-full bg-green-500 rounded-full" style={{ width: '64%' }}></div>
         </div>
      </div>

      <div className="flex flex-col xl:flex-row gap-8">
         {/* Main Grid */}
         <div className="flex-1">
            <div className="flex items-center justify-between mb-6">
               <h2 className="text-xl font-bold text-slate-900">Active Goals</h2>
               <button className="text-sm font-semibold text-slate-500 hover:text-green-600 transition-colors">View All</button>
            </div>
            
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
               {activeGoals.map(goal => (
                  <GoalCard key={goal.id} goal={goal} />
               ))}
               
               {/* Add New Card */}
               <button className="group bg-slate-50 rounded-2xl p-6 border-2 border-dashed border-slate-200 hover:border-green-400 hover:bg-green-50/30 transition-all duration-300 flex flex-col items-center justify-center gap-4 min-h-[240px]">
                  <div className="w-14 h-14 rounded-full bg-white group-hover:bg-green-100 flex items-center justify-center text-slate-300 group-hover:text-green-500 transition-colors">
                     <span className="material-symbols-outlined text-3xl">add</span>
                  </div>
                  <div className="text-center">
                     <h3 className="text-base font-bold text-slate-900 group-hover:text-green-700">Create New Goal</h3>
                     <p className="text-xs text-slate-400 mt-1">Start saving for something new</p>
                  </div>
               </button>
            </div>
         </div>

         {/* Sidebar History */}
         <div className="w-full xl:w-80 flex-shrink-0">
            <div className="bg-white rounded-2xl p-6 border border-slate-200 shadow-sm h-full">
               <h2 className="text-lg font-bold text-slate-900 mb-6">Contribution History</h2>
               <ContributionHistory />
            </div>
         </div>
      </div>
    </div>
  );
};

export default GoalsPage;