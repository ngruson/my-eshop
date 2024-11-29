namespace eShop.Customer.Contracts.GetCustomers;

public record CustomerNameDto(string FirstName, string LastName)
{
    public string FullName => $"{this.FirstName} {this.LastName}";
}
