using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget4U.Data;

/// <summary>
/// Represents a user-defined spending or income category (e.g., Food, Rent, Salary).
/// </summary>
public class Category
{
    [Key]
    public int Id { get; set; }

    /// <summary>Foreign key linking this category to its owner.</summary>
    [Required]
    public string UserId { get; set; } = string.Empty;

    /// <summary>Display name of the category.</summary>
    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Hex color code used for UI badges (e.g., "#e74c3c").</summary>
    [MaxLength(7)]
    public string Color { get; set; } = "#6c757d";

    /// <summary>Navigation: transactions belonging to this category.</summary>
    public ICollection<Transaction> Transactions { get; set; } = [];
}
