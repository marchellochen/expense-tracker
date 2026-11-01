using System.Globalization;
using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.Dtos;
using ExpenseTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ExpensesController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/expenses?month=2026-10&categoryId=3
    [HttpGet]
    public async Task<ActionResult<List<ExpenseDto>>> GetAll(
        [FromQuery] string? month,
        [FromQuery] int? categoryId)
    {
        var query = _db.Expenses
            .Include(e => e.Category)
            .AsQueryable();

        if (categoryId is not null)
        {
            query = query.Where(e => e.CategoryId == categoryId.Value);
        }

        if (month is not null)
        {
            var parsed = DateTime.TryParseExact(
                month,
                "yyyy-MM",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var start);

            if (!parsed)
            {
                return BadRequest(new { message = "month must look like 2026-10." });
            }

            var end = start.AddMonths(1);

            query = query.Where(e => e.Date >= start && e.Date < end);
        }

        var expenses = await query
            .OrderByDescending(e => e.Date)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return Ok(expenses.Select(ToDto).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseDto>> GetById(int id)
    {
        var expense = await _db.Expenses
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (expense is null)
        {
            return NotFound();
        }

        return Ok(ToDto(expense));
    }

    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> Create(SaveExpenseDto dto)
    {
        var category = await _db.Categories.FindAsync(dto.CategoryId);

        if (category is null)
        {
            return BadRequest(new { message = "Category does not exist." });
        }

        var expense = new Expense
        {
            Amount = dto.Amount,
            Date = dto.Date,
            Note = dto.Note,
            CategoryId = category.Id,
            Category = category
        };

        _db.Expenses.Add(expense);

        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = expense.Id }, ToDto(expense));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ExpenseDto>> Update(int id, SaveExpenseDto dto)
    {
        var expense = await _db.Expenses
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (expense is null)
        {
            return NotFound();
        }

        var category = await _db.Categories.FindAsync(dto.CategoryId);

        if (category is null)
        {
            return BadRequest(new { message = "Category does not exist." });
        }

        expense.Amount = dto.Amount;
        expense.Date = dto.Date;
        expense.Note = dto.Note;
        expense.CategoryId = category.Id;
        expense.Category = category;

        await _db.SaveChangesAsync();

        return Ok(ToDto(expense));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var expense = await _db.Expenses.FindAsync(id);

        if (expense is null)
        {
            return NotFound();
        }

        _db.Expenses.Remove(expense);

        await _db.SaveChangesAsync();

        return NoContent();
    }

    private static ExpenseDto ToDto(Expense expense) => new()
    {
        Id = expense.Id,
        Amount = expense.Amount,
        Date = expense.Date,
        Note = expense.Note,
        CategoryId = expense.CategoryId,
        CategoryName = expense.Category.Name,
        CategoryColor = expense.Category.Color
    };
}