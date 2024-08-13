using System.Diagnostics;
using Microsoft.SemanticKernel.Embeddings;
using Pgvector;

namespace eShop.Catalog.API.Services;

public sealed class CatalogAI(IWebHostEnvironment environment, ILogger<CatalogAI> logger, ITextEmbeddingGenerationService? embeddingGenerator = null) : ICatalogAI
{
    private const int EmbeddingDimensions = 384;
    private readonly ITextEmbeddingGenerationService? _embeddingGenerator = embeddingGenerator;

    /// <summary>The web host environment.</summary>
    private readonly IWebHostEnvironment _environment = environment;
    /// <summary>Logger for use in AI operations.</summary>
    private readonly ILogger _logger = logger;

    /// <inheritdoc/>
    public bool IsEnabled => this._embeddingGenerator is not null;

    /// <inheritdoc/>
    public ValueTask<Vector?> GetEmbeddingAsync(CatalogItem item) =>
        this.IsEnabled ?
            this.GetEmbeddingAsync(CatalogItemToString(item)) :
            ValueTask.FromResult<Vector?>(null);

    /// <inheritdoc/>
    public async ValueTask<IReadOnlyList<Vector>?> GetEmbeddingsAsync(IEnumerable<CatalogItem> items)
    {
        if (this.IsEnabled)
        {
            long timestamp = Stopwatch.GetTimestamp();

            IList<ReadOnlyMemory<float>> embeddings = await this._embeddingGenerator!.GenerateEmbeddingsAsync(items.Select(CatalogItemToString).ToList());
            var results = embeddings.Select(m => new Vector(m[0..EmbeddingDimensions])).ToList();

            if (this._logger.IsEnabled(LogLevel.Trace))
            {
                this._logger.LogTrace("Generated {EmbeddingsCount} embeddings in {ElapsedMilliseconds}s", results.Count, Stopwatch.GetElapsedTime(timestamp).TotalSeconds);
            }

            return results;
        }

        return null;
    }

    /// <inheritdoc/>
    public async ValueTask<Vector?> GetEmbeddingAsync(string text)
    {
        if (this.IsEnabled)
        {
            long timestamp = Stopwatch.GetTimestamp();

            ReadOnlyMemory<float> embedding = await this._embeddingGenerator!.GenerateEmbeddingAsync(text);
            embedding = embedding[0..EmbeddingDimensions];

            if (this._logger.IsEnabled(LogLevel.Trace))
            {
                this._logger.LogTrace("Generated embedding in {ElapsedMilliseconds}s: '{Text}'", Stopwatch.GetElapsedTime(timestamp).TotalSeconds, text);
            }

            return new Vector(embedding);
        }

        return null;
    }

    private static string CatalogItemToString(CatalogItem item) => $"{item.Name} {item.Description}";
}
