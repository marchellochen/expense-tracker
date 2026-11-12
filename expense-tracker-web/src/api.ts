import type {
  Category,
  Expense,
  SaveCategory,
  SaveExpense,
} from "./types";

const API_URL = import.meta.env.VITE_API_URL ?? "http://localhost:5254";

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const response = await fetch(`${API_URL}${path}`, {
    headers: {
      "Content-Type": "application/json",
    },
    ...options,
  });

  if (!response.ok) {
    throw new Error(`Request failed with status ${response.status}`);
  }

  // DELETE returns 204 No Content, so there is no JSON to parse
  if (response.status === 204) {
    return undefined as T;
  }

  return response.json() as Promise<T>;
}

export function getCategories() {
  return request<Category[]>("/api/categories");
}

export function createCategory(category: SaveCategory) {
  return request<Category>("/api/categories", {
    method: "POST",
    body: JSON.stringify(category),
  });
}

export function deleteCategory(id: number) {
  return request<void>(`/api/categories/${id}`, {
    method: "DELETE",
  });
}

export function getExpenses(month?: string, categoryId?: number) {
  const params = new URLSearchParams();

  if (month) {
    params.set("month", month);
  }

  if (categoryId) {
    params.set("categoryId", String(categoryId));
  }

  const query = params.toString();

  return request<Expense[]>(`/api/expenses${query ? `?${query}` : ""}`);
}

export function createExpense(expense: SaveExpense) {
  return request<Expense>("/api/expenses", {
    method: "POST",
    body: JSON.stringify(expense),
  });
}

export function deleteExpense(id: number) {
  return request<void>(`/api/expenses/${id}`, {
    method: "DELETE",
  });
}