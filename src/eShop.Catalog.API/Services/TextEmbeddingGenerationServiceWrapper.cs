using Microsoft.SemanticKernel.Embeddings;

namespace eShop.Catalog.API.Services;

public class TextEmbeddingGenerationServiceWrapper(ITextEmbeddingGenerationService? embeddingGenerator = null)
{
    private readonly ITextEmbeddingGenerationService? embeddingGenerator = embeddingGenerator;

    public virtual bool IsEnabled => this.embeddingGenerator is not null;

    public virtual Task<ReadOnlyMemory<float>> GenerateEmbeddingAsync(string value) =>
        this.embeddingGenerator is not null ?
            this.embeddingGenerator.GenerateEmbeddingAsync(value) :
            Task.FromResult(ReadOnlyMemory<float>.Empty);

    public virtual Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> value) =>
        this.embeddingGenerator is not null ?
            this.embeddingGenerator.GenerateEmbeddingsAsync(value) :
            Task.FromResult(new List<ReadOnlyMemory<float>>() as IList<ReadOnlyMemory<float>>);
}
