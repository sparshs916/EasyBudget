export interface Transaction {
  id: string;
  name: string;
  category: string;
  date: string;
  amount: number;
  type: 'income' | 'expense';
}

export interface Goal {
  id: string;
  title: string;
  targetDate: string;
  currentAmount: number;
  targetAmount: number;
  icon: string;
  colorClass: string;
}

export interface Contribution {
  id: string;
  title: string;
  date: string;
  amount: number;
  type: 'add' | 'milestone';
}

export interface BudgetCategory {
  id: string;
  name: string;
  spent: number;
  limit: number;
  type: 'fixed' | 'variable';
}