namespace eShop.Identity.Contracts.GetUsers;

public record UserDto(
    string Id,
    string UserName,
    string FirstName,
    string LastName);
