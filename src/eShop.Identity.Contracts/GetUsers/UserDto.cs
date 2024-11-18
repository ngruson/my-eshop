namespace eShop.Identity.Contracts.GetUsers;

public record UserDto(
    Guid Id,
    string UserName,
    string FirstName,
    string LastName);
