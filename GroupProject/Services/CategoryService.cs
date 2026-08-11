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

    /// <summary>
    /// Retrieves all categories created by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of Category objects sorted alphabetically by name.</returns>
    public async Task<List<Category>> GetCategoriesAsync(string userId)
    {
        return await _context.Categories
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific category by its ID, ensuring it belongs to the specified user.
    /// </summary>
    /// <param name="id">The category ID.</param>
    /// <param name="userId">The user ID for authorization.</param>
    /// <returns>The Category object if found; otherwise, null.</returns>
    public async Task<Category?> GetCategoryByIdAsync(int id, string userId)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
    }

    /// <summary>
    /// Adds a new category to the database.
    /// </summary>
    /// <param name="category">The category entity to add.</param>
    public async Task AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing category in the database.
    /// </summary>
    /// <param name="category">The category entity with updated values.</param>
    public async Task UpdateCategoryAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a category if it belongs to the specified user.
    /// </summary>
    /// <param name="id">The ID of the category to delete.</param>
    /// <param name="userId">The user ID for authorization.</param>
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

    /// <summary>
    /// Retrieves all budget limits set by a user for a specific month and year.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="month">The month (1-12).</param>
    /// <param name="year">The year.</param>
    /// <returns>A list of BudgetLimit objects.</returns>
    public async Task<List<BudgetLimit>> GetBudgetLimitsAsync(string userId, int month, int year)
    {
        return await _context.BudgetLimits
            .Include(b => b.Category)
            .Where(b => b.UserId == userId && b.Month == month && b.Year == year)
            .ToListAsync();
    }

    /// <summary>
    /// Sets or updates a budget limit for a specific category, month, and year.
    /// </summary>
    /// <param name="budgetLimit">The BudgetLimit entity containing the limit details.</param>
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
