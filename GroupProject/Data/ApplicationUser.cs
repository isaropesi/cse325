using Microsoft.AspNetCore.Identity;

namespace Budget4U.Data;

/// <summary>
/// Extends the default Identity user with an optional display name.
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
}
