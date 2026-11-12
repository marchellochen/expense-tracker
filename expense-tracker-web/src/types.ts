export interface Category {
  id: number;

  name: string;

  color: string;
}

export interface Expense {
  id: number;

  amount: number;

  // The API sends dates as ISO strings, e.g. "2026-11-01T00:00:00"
  date: string;

  note: string | null;

  categoryId: number;

  categoryName: string;

  categoryColor: string;
}

export interface SaveCategory {
  name: string;

  color: string;
}

export interface SaveExpense {
  amount: number;

  date: string;

  note: string | null;

  categoryId: number;
}