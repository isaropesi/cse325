using Budget4U.Data;
using Microsoft.EntityFrameworkCore;

namespace Budget4U.Services;

public class CategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetCategoriesAsync(string userId)
    {
        return await _context.Categories
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id, string userId)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
    }

    public async Task AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id, string userId)
    {
        var category = await GetCategoryByIdAsync(id, userId);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }

    // --- Budget Limits ---

    public async Task<List<BudgetLimit>> GetBudgetLimitsAsync(string userId, int month, int year)
    {
        return await _context.BudgetLimits
            .Include(b => b.Category)
            .Where(b => b.UserId == userId && b.Month == month && b.Year == year)
            .ToListAsync();
    }

    public async Task SetBudgetLimitAsync(BudgetLimit budgetLimit)
    {
        var existing = await _context.BudgetLimits
            .FirstOrDefaultAsync(b => b.UserId == budgetLimit.UserId && 
                                      b.CategoryId == budgetLimit.CategoryId && 
                                      b.Month == budgetLimit.Month && 
                                      b.Year == budgetLimit.Year);
                                      
        if (existing != null)
        {
            existing.LimitAmount = budgetLimit.LimitAmount;
            _context.BudgetLimits.Update(existing);
        }
        else
        {
            _context.BudgetLimits.Add(budgetLimit);
        }
        await _context.SaveChangesAsync();
    }
}
