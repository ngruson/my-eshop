using eShop.Ordering.FunctionalTests;

namespace Ordering.UnitTests;
public class ProgramUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task start_program(OrderingApiFixture fixture)
    {
        // Arrange
        
        bool result;

        // Act

        try
        {
            await fixture.InitializeAsync();
            result = true;
        }
        catch
        {
            result = false;
        }


        // Assert

        Assert.True(result);
    }
}
