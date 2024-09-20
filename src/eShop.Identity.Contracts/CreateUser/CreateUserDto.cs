namespace eShop.Identity.Contracts.CreateUser;

public record CreateUserDto(string UserName, string FirstName, string LastName, string Email, string PhoneNumber)
{
}
