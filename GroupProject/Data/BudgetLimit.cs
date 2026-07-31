using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget4U.Data;

/// <summary>
/// Represents a monthly spending cap set by a user for a specific category.
/// </summary>
public class BudgetLimit
{
    [Key]
    public int Id { get; set; }

    /// <summary>Foreign key linking this limit to its owner.</summary>
    [Required]
    public string UserId { get; set; } = string.Empty;

    /// <summary>Foreign key to the category being limited.</summary>
    [Required]
    public int CategoryId { get; set; }

    /// <summary>Navigation: the category being limited.</summary>
    public Category? Category { get; set; }

    /// <summary>Calendar month this limit applies to (1–12).</summary>
    [Required, Range(1, 12)]
    public int Month { get; set; }

    /// <summary>Calendar year this limit applies to.</summary>
    [Required]
    public int Year { get; set; }

    /// <summary>Maximum spending amount allowed for this category/month.</summary>
    [Required, Column(TypeName = "decimal(18,2)")]
    [Range(0.01, 999999.99, ErrorMessage = "Limit must be greater than zero")]
    public decimal LimitAmount { get; set; }
}
