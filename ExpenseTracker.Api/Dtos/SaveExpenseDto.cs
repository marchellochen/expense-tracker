using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.Dtos;

public class SaveExpenseDto
{
    [Range(typeof(decimal), "0.01", "9999999999", ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    [MaxLength(200)]
    public string? Note { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}