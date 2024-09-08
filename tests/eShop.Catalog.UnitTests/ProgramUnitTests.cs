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
            //CatalogApiFixture fixture = new();
            // fixture.WithWebHostBuilder(builder => builder.AddApplicationServices);

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

    //[Theory, AutoNSubstituteData]
    //public async Task start_program_with_ai_onnx(
    //    string embeddingModelPath,
    //    string embeddingVocalPath)
    //{
    //    // Arrange

    //    bool result;

    //    // Act
    //    try
    //    {
    //        Dictionary<string, string> config = new()
    //        {
    //            { "AI:Onnx:EmbeddingModelPath", embeddingModelPath },
    //            { "AI:Onnx:EmbeddingVocabPath", embeddingVocalPath }
    //        };

    //        CatalogApiFixture fixture = new(config);
    //        fixture.CreateDefaultClient();

    //        #pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    //        fixture.Services.GetRequiredService<ITextEmbeddingGenerationService>();
    //        #pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    //        await fixture.InitializeAsync();
    //        result = true;
    //    }
    //    catch
    //    {
    //        result = false;
    //    }

    //    // Assert

    //    Assert.True(result);
    //}
}
