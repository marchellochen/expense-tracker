namespace ExpenseTracker.Api.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = "#293F76";

    public List<Expense> Expenses { get; set; } = new();
}