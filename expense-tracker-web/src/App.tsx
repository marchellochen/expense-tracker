import { useEffect, useState } from "react";

import { getExpenses } from "./api";
import ExpenseList from "./ExpenseList";
import type { Expense } from "./types";

function getCurrentMonth() {
  const now = new Date();

  const month = String(now.getMonth() + 1).padStart(2, "0");

  return `${now.getFullYear()}-${month}`;
}

export default function App() {
  const [expenses, setExpenses] = useState<Expense[]>([]);

  const [loading, setLoading] = useState(true);

  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    getExpenses(getCurrentMonth())
      .then(setExpenses)
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false));
  }, []);

  return (
    <main>
      <h1>Expense Tracker</h1>

      {loading && <p>Loading...</p>}

      {error && <p>Something went wrong: {error}</p>}

      {!loading && !error && <ExpenseList expenses={expenses} />}
    </main>
  );
}