import React from 'react';

interface StatCardProps {
  title: string;
  icon: string;
  iconColor: string;
  mainValue: string;
  mainValueColor?: string;
  subValue: string;
  actionText: string;
  actionColor: string;
}

const StatCard: React.FC<StatCardProps> = ({ 
  title, icon, iconColor, mainValue, mainValueColor = 'text-slate-900', subValue, actionText, actionColor 
}) => {
  return (
    <div className="bg-white rounded-2xl p-6 border border-slate-100 shadow-sm flex flex-col justify-between h-full hover:shadow-md transition-shadow">
      <div className="flex items-center justify-between mb-4">
        <h3 className="font-bold text-slate-800 text-sm lg:text-base">{title}</h3>
        <span className={`material-symbols-outlined ${iconColor} text-2xl`}>{icon}</span>
      </div>
      
      <div className="flex flex-col items-center justify-center mb-6 text-center">
        <p className={`text-2xl lg:text-3xl font-extrabold tracking-tight ${mainValueColor}`}>{mainValue}</p>
        <p className="text-sm text-slate-400 mt-2">{subValue}</p>
      </div>

      <div className="text-center mt-auto">
        <button className={`text-xs font-bold ${actionColor} hover:underline`}>
          {actionText}
        </button>
      </div>
    </div>
  );
};

export default StatCard;