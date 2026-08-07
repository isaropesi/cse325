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

    public async Task<Transaction?> GetTransactionByIdAsync(int id, string userId)
    {
        return await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTransactionAsync(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTransactionAsync(int id, string userId)
    {
        var transaction = await GetTransactionByIdAsync(id, userId);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Transaction>> GetRecentTransactionsAsync(string userId, int count)
    {
        return await _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date)
            .Take(count)
            .ToListAsync();
    }

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
