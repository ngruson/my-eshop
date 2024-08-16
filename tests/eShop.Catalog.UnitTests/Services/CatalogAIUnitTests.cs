using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Pgvector;

namespace eShop.Catalog.UnitTests.Services;
public class CatalogAIUnitTests
{
    public class GetEmbedding
    {
        [Theory, AutoNSubstituteData]
        internal async Task when_enabled_return_vector(
            [Substitute, Frozen] TextEmbeddingGenerationServiceWrapper wrapper,
            CatalogAI catalogAI,
            CatalogItem catalogItem)
        {
            // Arrange

            wrapper.IsEnabled.Returns(true);

            wrapper.GenerateEmbeddingAsync(Arg.Any<string>())
                .Returns(new ReadOnlyMemory<float>(new float[384]));

            // Act

            Vector? vector = await catalogAI.GetEmbeddingAsync(catalogItem);

            // Assert

            Assert.NotNull(vector);
        }

        [Theory, AutoNSubstituteData]
        internal async Task when_enabled_and_trace_log_level_return_vector(
            [Substitute, Frozen] TextEmbeddingGenerationServiceWrapper wrapper,
            [Substitute, Frozen] ILogger<CatalogAI> logger,
            CatalogAI catalogAI,
            CatalogItem catalogItem)
        {
            // Arrange

            wrapper.IsEnabled.Returns(true);

            wrapper.GenerateEmbeddingAsync(Arg.Any<string>())
                .Returns(new ReadOnlyMemory<float>(new float[384]));

            logger.IsEnabled(LogLevel.Trace)
                .Returns(true);

            // Act

            Vector? vector = await catalogAI.GetEmbeddingAsync(catalogItem);

            // Assert

            Assert.NotNull(vector);
        }

        [Theory, AutoNSubstituteData]
        internal async Task when_disabled_return_null(
            [Substitute, Frozen] TextEmbeddingGenerationServiceWrapper wrapper,
            CatalogAI catalogAI,
            CatalogItem catalogItem)
        {
            // Arrange

            wrapper.IsEnabled.Returns(false);

            // Act

            Vector? vector = await catalogAI.GetEmbeddingAsync(catalogItem);

            // Assert

            Assert.Null(vector);
        }
    }

    public class GetEmbeddings
    {
        [Theory, AutoNSubstituteData]
        internal async Task when_enabled_return_vectors(
            [Substitute, Frozen] TextEmbeddingGenerationServiceWrapper wrapper,
            CatalogAI catalogAI,
            List<CatalogItem> catalogItems)
        {
            // Arrange

            wrapper.IsEnabled.Returns(true);

            wrapper.GenerateEmbeddingsAsync(Arg.Any<IList<string>>())
                .Returns([new ReadOnlyMemory<float>(new float[384])]);

            // Act

            IReadOnlyList<Vector>? vectors = await catalogAI.GetEmbeddingsAsync(catalogItems);

            // Assert

            Assert.NotNull(vectors);
        }

        [Theory, AutoNSubstituteData]
        internal async Task when_enabled_and_trace_log_level_return_vector(
            [Substitute, Frozen] TextEmbeddingGenerationServiceWrapper wrapper,
            [Substitute, Frozen] ILogger<CatalogAI> logger,
            CatalogAI catalogAI,
            List<CatalogItem> catalogItems)
        {
            // Arrange

            wrapper.IsEnabled.Returns(true);

            wrapper.GenerateEmbeddingsAsync(Arg.Any<IList<string>>())
                .Returns([new ReadOnlyMemory<float>(new float[384])]);

            logger.IsEnabled(LogLevel.Trace)
                .Returns(true);

            // Act

            IReadOnlyList<Vector>? vectors = await catalogAI.GetEmbeddingsAsync(catalogItems);

            // Assert

            Assert.NotNull(vectors);
        }

        [Theory, AutoNSubstituteData]
        internal async Task when_disabled_return_vectors(
            [Substitute, Frozen] TextEmbeddingGenerationServiceWrapper wrapper,
            CatalogAI catalogAI,
            List<CatalogItem> catalogItems)
        {
            // Arrange

            wrapper.IsEnabled.Returns(false);

            // Act

            IReadOnlyList<Vector>? vectors = await catalogAI.GetEmbeddingsAsync(catalogItems);

            // Assert

            Assert.Null(vectors);
        }
    }
}
