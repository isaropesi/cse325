using Budget4U.Data;
using Microsoft.EntityFrameworkCore;

namespace Budget4U.Services;

public class TransactionService
{
    private readonly ApplicationDbContext _context;

    public TransactionService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a list of transactions for a specific user, optionally filtered by month and year.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="month">Optional month filter (1-12).</param>
    /// <param name="year">Optional year filter.</param>
    /// <returns>A list of Transaction objects sorted by date descending.</returns>
    public async Task<List<Transaction>> GetTransactionsAsync(string userId, int? month = null, int? year = null)
    {
        var query = _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId);

        if (month.HasValue && year.HasValue)
        {
            query = query.Where(t => t.Date.Month == month.Value && t.Date.Year == year.Value);
        }

        return await query.OrderByDescending(t => t.Date).ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific transaction by its ID, ensuring it belongs to the user.
    /// </summary>
    /// <param name="id">The transaction ID.</param>
    /// <param name="userId">The user ID for authorization.</param>
    /// <returns>The Transaction object if found; otherwise, null.</returns>
    public async Task<Transaction?> GetTransactionByIdAsync(int id, string userId)
    {
        return await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }

    /// <summary>
    /// Adds a new transaction to the database.
    /// </summary>
    /// <param name="transaction">The transaction entity to add.</param>
    public async Task AddTransactionAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing transaction in the database.
    /// </summary>
    /// <param name="transaction">The transaction entity with updated values.</param>
    public async Task UpdateTransactionAsync(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a transaction if it belongs to the specified user.
    /// </summary>
    /// <param name="id">The ID of the transaction to delete.</param>
    /// <param name="userId">The user ID for authorization.</param>
    public async Task DeleteTransactionAsync(int id, string userId)
    {
        var transaction = await GetTransactionByIdAsync(id, userId);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Retrieves the most recent transactions for a user up to a specified count.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="count">The maximum number of transactions to return.</param>
    /// <returns>A list of recent Transaction objects.</returns>
    public async Task<List<Transaction>> GetRecentTransactionsAsync(string userId, int count)
    {
        return await _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date)
            .Take(count)
            .ToListAsync();
    }

    /// <summary>
    /// Calculates the total income and total expense for a specific month and year.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="month">The month to calculate (1-12).</param>
    /// <param name="year">The year to calculate.</param>
    /// <returns>A tuple containing the total Income and total Expense amounts.</returns>
    public async Task<(decimal Income, decimal Expense)> GetMonthlyTotalsAsync(string userId, int month, int year)
    {
        var transactions = await _context.Transactions
            .Where(t => t.UserId == userId && t.Date.Month == month && t.Date.Year == year)
            .ToListAsync();

        var income = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
        var expense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);

        return (income, expense);
    }
}
