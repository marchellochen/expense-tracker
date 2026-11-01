using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.Dtos;
using ExpenseTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _db;

    public CategoriesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAll()
    {
        var categories = await _db.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();

        return Ok(categories.Select(ToDto).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var category = await _db.Categories.FindAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        return Ok(ToDto(category));
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(SaveCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name.Trim(),
            Color = dto.Color
        };

        _db.Categories.Add(category);

        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = category.Id }, ToDto(category));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> Update(int id, SaveCategoryDto dto)
    {
        var category = await _db.Categories.FindAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        category.Name = dto.Name.Trim();
        category.Color = dto.Color;

        await _db.SaveChangesAsync();

        return Ok(ToDto(category));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _db.Categories.FindAsync(id);

        if (category is null)
        {
            return NotFound();
        }

        var hasExpenses = await _db.Expenses.AnyAsync(e => e.CategoryId == id);

        if (hasExpenses)
        {
            return Conflict(new { message = "This category still has expenses. Move or delete them first." });
        }

        _db.Categories.Remove(category);

        await _db.SaveChangesAsync();

        return NoContent();
    }

    private static CategoryDto ToDto(Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Color = category.Color
    };
}