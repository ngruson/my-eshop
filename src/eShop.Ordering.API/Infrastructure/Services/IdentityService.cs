namespace eShop.Ordering.API.Infrastructure.Services;

public class IdentityService(IHttpContextAccessor context) : IIdentityService
{
    public Guid? GetUserIdentity()
    {
        string? subValue = context.HttpContext?.User.FindFirst("sub")?.Value;

        if (subValue is not null)
        {
            return Guid.Parse(subValue);
        }

        return null;
    }

    public string? GetUserName()
        => context.HttpContext?.User.Identity?.Name;
}
