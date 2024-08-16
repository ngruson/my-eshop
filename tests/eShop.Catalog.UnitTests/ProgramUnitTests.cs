using eShop.Catalog.FunctionalTests;

namespace eShop.Catalog.UnitTests;

public class ProgramUnitTests
{
    [Theory, AutoNSubstituteData]
    public async Task start_program(CatalogApiFixture fixture)
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
