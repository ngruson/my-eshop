using eShop.Shared.Data;

namespace eShop.Shared.UnitTests.Data;

public class ValueObjectUnitTests
{
    [Theory, AutoNSubstituteData]
    internal void two_objects_are_equal(
        TestValueObject sut)
    {
        // Arrange

        TestValueObject copy = sut;

        // Act

        bool equal = sut == copy;

        // Assert

        Assert.True(equal);
    }

    [Theory, AutoNSubstituteData]
    internal void two_objects_are_not_equal(
        TestValueObject sut,
        TestValueObject compare)
    {
        // Arrange

        // Act

        bool equal = sut == compare;

        // Assert

        Assert.False(equal);
    }

    [Theory, AutoNSubstituteData]
    internal void get_hash_code(
        TestValueObject sut)
    {
        // Arrange

        int x = sut.FirstName.GetHashCode();
        int y = sut.LastName.GetHashCode();

        // Act

        int result = sut.GetHashCode();

        // Assert

        Assert.Equal(x ^ y, result);
    }

    [Theory, AutoNSubstituteData]
    internal void get_copy(
        TestValueObject sut)
    {
        // Arrange

        // Act

        ValueObject? copy = sut.GetCopy();

        // Assert

        Assert.Equal(sut, copy);
    }
}
