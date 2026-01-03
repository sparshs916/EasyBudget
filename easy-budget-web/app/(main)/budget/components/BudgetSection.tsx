import React from 'react';

interface SimpleItem {
  label: string;
  amount: string;
  status: 'paid' | 'pending';
  accent?: boolean;
}

interface ProgressItem {
  label: string;
  current: number;
  max: number;
  color: string;
  alert?: string;
  badge?: string;
  subtext?: string;
}

interface Props {
  title: string;
  subtitle: string;
  icon: string;
  iconColor: string;
  bgColor: string;
  totalLabel: string;
  totalValue: string;
  totalColor: string;
  manageText: string;
  items?: SimpleItem[];
  progressItems?: ProgressItem[];
}

const BudgetSection: React.FC<Props> = ({
  title, subtitle, icon, iconColor, bgColor,
  totalLabel, totalValue, totalColor, manageText,
  items, progressItems
}) => {
  return (
    <div className="bg-white rounded-2xl p-6 border border-slate-200 shadow-sm flex flex-col h-full">
      {/* Header */}
      <div className="flex justify-between items-start mb-6">
        <div>
          <h4 className="text-lg font-bold text-slate-900">{title}</h4>
          <p className="text-xs text-slate-400 mt-1">{subtitle}</p>
        </div>
        <div className={`p-2 rounded-lg ${bgColor}`}>
          <span className={`material-symbols-outlined ${iconColor}`}>{icon}</span>
        </div>
      </div>

      {/* Content */}
      <div className="flex-1 space-y-4 mb-6">
        {items && items.map((item, idx) => (
          <div 
            key={idx} 
            className={`
              p-3 rounded-lg flex items-center justify-between
              ${item.accent ? 'bg-white border-l-4 border-red-500 shadow-sm' : 'bg-slate-50/50 opacity-80 hover:opacity-100'}
            `}
          >
            <div className="flex items-center gap-3">
              {item.status === 'paid' ? (
                 <span className="material-symbols-outlined text-green-500 text-lg">check_circle</span>
              ) : (
                 <span className={`material-symbols-outlined text-lg ${item.accent ? 'text-red-500 animate-pulse' : 'text-slate-400'}`}>pending</span>
              )}
              <span className={`text-sm ${item.status === 'paid' ? 'text-slate-400 line-through' : 'font-medium text-slate-800'}`}>
                {item.label}
              </span>
            </div>
            <div className="flex items-center gap-2">
              <span className={`font-bold text-sm ${item.accent ? 'text-red-500' : 'text-slate-400'}`}>{item.amount}</span>
              {item.status === 'pending' && (
                 <button className="w-6 h-6 rounded-full bg-red-50 text-red-500 flex items-center justify-center hover:bg-red-100 transition-colors">
                    <span className="material-symbols-outlined text-[14px]">task_alt</span>
                 </button>
              )}
            </div>
          </div>
        ))}

        {progressItems && progressItems.map((item, idx) => {
          const percent = item.max > 0 ? (item.current / item.max) * 100 : 0;
          return (
            <div key={idx} className="bg-slate-50 p-4 rounded-lg flex flex-col gap-2">
               <div className="flex items-center justify-between">
                  <span className="text-sm font-bold text-slate-800">{item.label}</span>
                  {item.badge && <span className="text-xs font-bold text-blue-500">{item.badge}</span>}
               </div>
               
               {item.subtext ? (
                 <div className="flex justify-between text-xs text-slate-500 mb-1">
                    <span>{item.subtext}</span>
                    <span className={item.current > 0 ? 'text-blue-500 font-bold' : ''}>Current: ${item.current}</span>
                 </div>
               ) : (
                 <div className="flex justify-between text-xs items-end">
                    <div className="w-full bg-slate-200 rounded-full h-2.5 overflow-hidden mr-2">
                       <div className={`${item.color} h-full rounded-full`} style={{ width: `${percent}%` }}></div>
                    </div>
                    <span className="whitespace-nowrap text-slate-400">
                       <span className={`font-bold ${totalColor}`}>{item.current}</span> / {item.max}
                    </span>
                 </div>
               )}

               {item.subtext && (
                 <div className="w-full bg-slate-200 rounded-full h-2.5 overflow-hidden">
                    <div className={`${item.color} h-full rounded-full`} style={{ width: `${percent}%` }}></div>
                 </div>
               )}

               {item.alert && (
                 <p className="text-[10px] text-red-500 font-bold text-right">{item.alert}</p>
               )}
            </div>
          )
        })}
      </div>

      {/* Footer */}
      <div className="mt-auto pt-4 border-t border-slate-100">
         <div className="flex items-center justify-between mb-3">
            <span className="font-bold text-sm text-slate-800">{totalLabel}:</span>
            <span className={`font-bold text-lg ${totalColor}`}>{totalValue}</span>
         </div>
         <button className={`w-full flex items-center justify-center gap-2 py-2.5 rounded-lg ${bgColor} bg-opacity-50 hover:bg-opacity-80 transition-colors ${totalColor} text-sm font-bold`}>
            <span className="material-symbols-outlined text-[18px]">edit_square</span>
            {manageText}
         </button>
      </div>
    </div>
  );
};

export default BudgetSection;