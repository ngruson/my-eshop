using eShop.Shared.Data;

namespace eShop.Identity.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser, IAggregateRoot
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
