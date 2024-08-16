using eShop.Catalog.FunctionalTests;

namespace eShop.Catalog.UnitTests;

public class ProgramUnitTests
{
    [Fact]
    public async Task start_program()
    {
        // Arrange

        bool result;

        // Act
        try
        {
            CatalogApiFixture fixture = new();
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

    [Theory, AutoNSubstituteData]
    public async Task start_program_with_ai_onnx(
        string embeddingModelPath,
        string embeddingVocalPath)
    {
        // Arrange

        bool result;

        // Act
        try
        {
            Dictionary<string, string> config = new()
            {
                { "AI:Onnx:EmbeddingModelPath", embeddingModelPath },
                { "AI:Onnx:EmbeddingVocabPath", embeddingVocalPath }
            };

            CatalogApiFixture fixture = new(config);

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
