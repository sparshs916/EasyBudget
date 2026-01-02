import React from 'react';
import { Transaction } from '../../types';

const transactions: Transaction[] = [
  { id: '1', name: 'Starbucks Coffee', category: 'Food & Drink', date: 'Oct 24, 2023', amount: 5.40, type: 'expense' },
  { id: '2', name: 'Electric Bill', category: 'Utilities', date: 'Oct 23, 2023', amount: 120.00, type: 'expense' },
  { id: '3', name: 'Direct Deposit', category: 'Income', date: 'Oct 15, 2023', amount: 2500.00, type: 'income' },
  { id: '4', name: 'Grocery Store', category: 'Groceries', date: 'Oct 12, 2023', amount: 45.00, type: 'expense' },
  { id: '5', name: 'Netflix', category: 'Subscription', date: 'Oct 10, 2023', amount: 15.99, type: 'expense' },
  { id: '6', name: 'Gym Membership', category: 'Health', date: 'Oct 05, 2023', amount: 45.00, type: 'expense' },
  { id: '7', name: 'Coffee Shop', category: 'Food & Drink', date: 'Oct 03, 2023', amount: 3.75, type: 'expense' },
  { id: '8', name: 'Salary', category: 'Income', date: 'Sep 30, 2023', amount: 2500.00, type: 'income' },
  { id: '9', name: 'Internet Bill', category: 'Utilities', date: 'Sep 28, 2023', amount: 70.00, type: 'expense' },
];

const RecentTransactions: React.FC = () => {
  return (
    <div className="flex flex-col gap-4 h-full">
      <div className="flex items-center justify-between">
         <h3 className="text-lg font-bold text-slate-800">Recent Transactions</h3>
         <button className="p-2 rounded-lg bg-white border border-slate-200 hover:bg-slate-50 text-slate-500">
            <span className="material-symbols-outlined text-[20px]">tune</span>
         </button>
      </div>

      <div className="relative">
         <input 
            type="text" 
            placeholder="Search transactions..." 
            className="w-full h-10 pl-10 pr-4 rounded-xl border border-slate-200 bg-white text-sm focus:ring-2 focus:ring-green-500 focus:outline-none"
         />
         <span className="material-symbols-outlined absolute left-3 top-2.5 text-slate-400 text-[20px]">search</span>
      </div>

      <div className="bg-white rounded-2xl border border-slate-200 shadow-sm flex-1 overflow-hidden flex flex-col">
         <div className="flex-1 overflow-y-auto no-scrollbar">
            {transactions.map((tx) => (
              <div key={tx.id} className="flex items-center justify-between p-4 border-b border-slate-50 hover:bg-slate-50 cursor-pointer transition-colors group">
                 <div className="flex-1 min-w-0 pr-3">
                    <p className="text-sm font-bold text-slate-800 truncate">{tx.name}</p>
                    <p className="text-xs text-slate-500 truncate">{tx.date} · {tx.category}</p>
                 </div>
                 <div className="flex items-center gap-2">
                    <span className={`text-sm font-bold whitespace-nowrap ${tx.type === 'income' ? 'text-green-600' : 'text-slate-900'}`}>
                       {tx.type === 'income' ? '+' : '-'}${tx.amount.toFixed(2)}
                    </span>
                    <span className="material-symbols-outlined text-slate-300 text-[18px] group-hover:text-green-500 transition-colors">chevron_right</span>
                 </div>
              </div>
            ))}
         </div>
         <div className="p-4 border-t border-slate-100 bg-slate-50/50">
            <button className="w-full flex items-center justify-center gap-2 h-10 rounded-lg bg-white border border-slate-200 hover:border-green-300 text-slate-700 text-sm font-bold shadow-sm transition-all">
               <span className="material-symbols-outlined text-[18px]">list_alt</span>
               View All Transactions
            </button>
         </div>
      </div>
    </div>
  );
};

export default RecentTransactions;