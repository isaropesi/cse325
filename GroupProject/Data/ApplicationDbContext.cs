using Budget4U.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Budget4U.Data;

/// <summary>
/// Main EF Core database context for Budget4U.
/// Extends IdentityDbContext to include all ASP.NET Core Identity tables.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    /// <summary>User-defined spending/income categories.</summary>
    public DbSet<Category> Categories { get; set; }

    /// <summary>All income and expense transactions logged by users.</summary>
    public DbSet<Transaction> Transactions { get; set; }

    /// <summary>Monthly budget limits per category per user.</summary>
    public DbSet<BudgetLimit> BudgetLimits { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Ensure UserId is required on all user-owned entities
        builder.Entity<Transaction>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Category>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<BudgetLimit>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
