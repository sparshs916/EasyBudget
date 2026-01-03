import React from 'react';
import { Goal } from '../../types';

interface Props {
  goal: Goal;
}

const GoalCard: React.FC<Props> = ({ goal }) => {
  const percentage = Math.min(100, Math.round((goal.currentAmount / goal.targetAmount) * 100));

  return (
    <div className="bg-white rounded-2xl p-5 border border-slate-200 shadow-sm hover:shadow-md transition-shadow flex flex-col gap-4">
       <div className="flex justify-between items-start">
          <div className={`w-12 h-12 rounded-xl flex items-center justify-center ${goal.colorClass}`}>
             <span className="material-symbols-outlined">{goal.icon}</span>
          </div>
          <button className="text-slate-300 hover:text-slate-500">
             <span className="material-symbols-outlined text-[20px]">more_vert</span>
          </button>
       </div>
       
       <div>
          <h3 className="text-lg font-bold text-slate-900 mb-1">{goal.title}</h3>
          <p className="text-xs text-slate-500">Target: {goal.targetDate}</p>
       </div>

       <div className="flex flex-col gap-2 mt-2">
          <div className="flex justify-between text-sm">
             <span className="font-bold text-slate-900">${goal.currentAmount.toLocaleString()}</span>
             <span className="text-slate-400">of ${Math.round(goal.targetAmount / 1000)}k</span>
          </div>
          <div className="h-2.5 w-full bg-slate-100 rounded-full overflow-hidden">
             <div className="h-full bg-green-500 rounded-full" style={{ width: `${percentage}%` }}></div>
          </div>
       </div>

       <button className="mt-2 w-full py-2.5 rounded-xl bg-slate-50 text-slate-700 text-sm font-bold hover:bg-green-500 hover:text-white transition-all">
          Add Funds
       </button>
    </div>
  );
};

export default GoalCard;