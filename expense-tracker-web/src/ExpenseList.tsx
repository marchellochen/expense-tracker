import type { Expense } from "./types";

interface ExpenseListProps {
  expenses: Expense[];
}

const currencyFormatter = new Intl.NumberFormat("id-ID", {
  style: "currency",
  currency: "IDR",
  maximumFractionDigits: 0,
});

export default function ExpenseList({ expenses }: ExpenseListProps) {
  if (expenses.length === 0) {
    return <p>No expenses for this month yet.</p>;
  }

  return (
    <ul>
      {expenses.map((expense) => (
        <li key={expense.id}>
          <span
            style={{
              display: "inline-block",
              width: "0.75rem",
              height: "0.75rem",
              borderRadius: "50%",
              backgroundColor: expense.categoryColor,
              marginRight: "0.5rem",
            }}
          />

          <strong>{expense.categoryName}</strong>

          {" · "}

          {currencyFormatter.format(expense.amount)}

          {" · "}

          {new Date(expense.date).toLocaleDateString()}

          {expense.note ? ` · ${expense.note}` : ""}
        </li>
      ))}
    </ul>
  );
}