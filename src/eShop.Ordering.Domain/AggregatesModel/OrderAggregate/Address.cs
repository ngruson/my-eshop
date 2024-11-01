using eShop.Shared.Data;

namespace eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

public class Address(string street, string city, string state, string country, string zipCode) : ValueObject
{
    public string Street { get; private set; } = street;
    public string City { get; private set; } = city;
    public string State { get; private set; } = state;
    public string Country { get; private set; } = country;
    public string ZipCode { get; private set; } = zipCode;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        // Using a yield return statement to return each element one at a time
        yield return this.Street;
        yield return this.City;
        yield return this.State;
        yield return this.Country;
        yield return this.ZipCode;
    }
}
