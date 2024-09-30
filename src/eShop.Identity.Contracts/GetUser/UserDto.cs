namespace eShop.Identity.Contracts.GetUser;

public record UserDto(
    string Id,
    string UserName,
    string FirstName,
    string LastName);
