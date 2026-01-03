import React from 'react';
import { Contribution } from '../../types';

const history: Contribution[] = [
  { id: '1', title: 'Added to Japan Trip', date: 'Today, 10:23 AM', amount: 200, type: 'add' },
  { id: '2', title: 'Monthly Auto-save', date: 'Yesterday', amount: 50, type: 'add' },
  { id: '3', title: 'Goal Reached!', date: 'Concert Tickets', amount: 0, type: 'milestone' },
  { id: '4', title: 'Bonus Deposit', date: 'Oct 24', amount: 1000, type: 'add' },
];

const ContributionHistory: React.FC = () => {
  return (
    <div className="flex flex-col gap-0">
      {history.map((item, idx) => (
        <React.Fragment key={item.id}>
           <div className="flex gap-3 items-start py-4">
              <div className={`mt-0.5 h-8 w-8 rounded-full flex items-center justify-center flex-shrink-0 ${item.type === 'milestone' ? 'bg-blue-50 text-blue-500' : 'bg-green-100 text-green-600'}`}>
                 <span className="material-symbols-outlined text-[18px]">
                    {item.type === 'milestone' ? 'celebration' : 'arrow_upward'}
                 </span>
              </div>
              <div className="flex flex-col flex-1 min-w-0">
                 <p className="text-sm font-bold text-slate-800 truncate">{item.title}</p>
                 <p className="text-xs text-slate-500 truncate">{item.date}</p>
              </div>
              {item.amount > 0 && (
                 <p className="text-sm font-bold text-green-600">+{item.amount.toLocaleString()}</p>
              )}
           </div>
           {idx < history.length - 1 && <div className="h-px bg-slate-100 w-full"></div>}
        </React.Fragment>
      ))}
      
      <button className="w-full mt-6 text-sm text-slate-400 font-medium hover:text-slate-800 transition-colors">
         View Full History
      </button>
    </div>
  );
};

export default ContributionHistory;