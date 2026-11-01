using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.Dtos;

public class SaveCategoryDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [RegularExpression("^#[0-9A-Fa-f]{6}$", ErrorMessage = "Color must look like #293F76.")]
    public string Color { get; set; } = "#293F76";
}