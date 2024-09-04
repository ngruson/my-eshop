using eShop.Shared.Data;

namespace eShop.Shared.UnitTests.Data;

internal class TestValueObject(string firstName, string lastName) : ValueObject
{
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.FirstName;
        yield return this.LastName;
    }
}
