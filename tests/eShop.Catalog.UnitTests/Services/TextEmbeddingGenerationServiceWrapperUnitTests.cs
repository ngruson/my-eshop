using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Catalog.API.Services;
using Microsoft.SemanticKernel.Embeddings;
using NSubstitute;

namespace eShop.Catalog.UnitTests.Services;

public class TextEmbeddingGenerationServiceWrapperUnitTests
{
    public class GenerateEmbedding
    {
        [Theory, AutoNSubstituteData]
        internal async Task when_enabled_return_embedding(
            #pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            [Substitute, Frozen] ITextEmbeddingGenerationService textEmbeddingGenerationService,
            #pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            TextEmbeddingGenerationServiceWrapper sut,
            string value,
            List<ReadOnlyMemory<float>> embeddings
        )
        {
            // Arrange

            textEmbeddingGenerationService.GenerateEmbeddingsAsync(Arg.Any<IList<string>>(), null, default)
                .Returns(embeddings);

            // Act

            var result = await sut.GenerateEmbeddingAsync(value);

            // Assert

            Assert.Equal(embeddings[0], result);

            #pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            await textEmbeddingGenerationService.Received().GenerateEmbeddingsAsync(Arg.Any<IList<string>>(), null, default);
            #pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        }

        [Theory, AutoNSubstituteData]
        internal async Task when_disabled_return_empty_value(
            #pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            [Substitute, Frozen] ITextEmbeddingGenerationService textEmbeddingGenerationService,
            #pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            string value
        )
        {
            // Arrange

            TextEmbeddingGenerationServiceWrapper sut = new(null);

            // Act

            var result = await sut.GenerateEmbeddingAsync(value);

            // Assert

            Assert.True(result.IsEmpty);

            #pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            await textEmbeddingGenerationService.DidNotReceive().GenerateEmbeddingsAsync(Arg.Any<IList<string>>(), null, default);
            #pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        }
    }

    public class GenerateEmbeddings
    {
        [Theory, AutoNSubstituteData]
        internal async Task when_enabled_return_embedding(
            #pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            [Substitute, Frozen] ITextEmbeddingGenerationService textEmbeddingGenerationService,
            #pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            TextEmbeddingGenerationServiceWrapper sut,
            List<string> value,
            List<ReadOnlyMemory<float>> embeddings
        )
        {
            // Arrange

            textEmbeddingGenerationService.GenerateEmbeddingsAsync(Arg.Any<IList<string>>(), null, default)
                .Returns(embeddings);

            // Act

            var result = await sut.GenerateEmbeddingsAsync(value);

            // Assert

            Assert.Equal(embeddings, result);

            #pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            await textEmbeddingGenerationService.Received().GenerateEmbeddingsAsync(Arg.Any<IList<string>>(), null, default);
            #pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        }

        [Theory, AutoNSubstituteData]
        internal async Task when_disabled_return_empty_value(
            #pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            [Substitute, Frozen] ITextEmbeddingGenerationService textEmbeddingGenerationService,
            #pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            List<string> value
        )
        {
            // Arrange

            TextEmbeddingGenerationServiceWrapper sut = new(null);

            // Act

            var result = await sut.GenerateEmbeddingsAsync(value);

            // Assert

            Assert.Empty(result);

            #pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            await textEmbeddingGenerationService.DidNotReceive().GenerateEmbeddingsAsync(Arg.Any<IList<string>>(), null, default);
            #pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        }
    }
}
