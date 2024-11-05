namespace eShop.Ordering.API.Infrastructure.Services;

public interface IIdentityService
{
    Guid? GetUserIdentity();

    string? GetUserName();
}

