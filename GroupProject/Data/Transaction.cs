using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget4U.Data;

/// <summary>Transaction type: income adds to balance, expense subtracts.</summary>
public enum TransactionType { Income, Expense }

/// <summary>
/// Represents a single financial entry (income or expense) logged by a user.
/// </summary>
public class Transaction
{
    [Key]
    public int Id { get; set; }

    /// <summary>Foreign key linking this transaction to its owner.</summary>
    [Required]
    public string UserId { get; set; } = string.Empty;

    /// <summary>Amount of money (always positive; type determines direction).</summary>
    [Required, Column(TypeName = "decimal(18,2)")]
    [Range(0.01, 999999.99, ErrorMessage = "Amount must be between R$0.01 and R$999,999.99")]
    public decimal Amount { get; set; }

    /// <summary>Date the transaction occurred.</summary>
    [Required]
    public DateTime Date { get; set; } = DateTime.Today;

    /// <summary>Short description of the transaction.</summary>
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    /// <summary>Whether this is income or an expense.</summary>
    [Required]
    public TransactionType Type { get; set; } = TransactionType.Expense;

    /// <summary>Foreign key to the category this transaction belongs to.</summary>
    public int? CategoryId { get; set; }

    /// <summary>Navigation: the category this transaction belongs to.</summary>
    public Category? Category { get; set; }
}
