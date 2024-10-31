using Ardalis.Result;
using eShop.MasterData.Contracts;
using eShop.ServiceInvocation.MasterDataApiClient;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace eShop.AdminApp.Application.Queries.MasterData.GetStates;

public class GetStatesQueryHandler(
    ILogger<GetStatesQueryHandler> logger,
    IMasterDataApiClient masterDataApiClient,
    IMemoryCache cache) : IRequestHandler<GetStatesQuery, Result<StateViewModel[]>>
{
    private readonly ILogger<GetStatesQueryHandler> logger = logger;
    private readonly IMasterDataApiClient masterDataApiClient = masterDataApiClient;
    private readonly IMemoryCache cache = cache;

    public async Task<Result<StateViewModel[]>> Handle(GetStatesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Retrieving states from cache or master data API.");

            StateViewModel[] viewModel;
            string key = "States";
            if (!this.cache.TryGetValue(key, out object? states))
            {
                this.logger.LogInformation("States not found in cache. Retrieving from master data API.");
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1));
                StateDto[] dto = await this.masterDataApiClient.GetStates();
                viewModel = [.. dto.MapToStateViewModels()];
                this.cache.Set(key, viewModel, cacheEntryOptions);
            }
            else
            {
                viewModel = (StateViewModel[])states!;
            }
            
            this.logger.LogInformation("States retrieved successfully.");

            return viewModel;
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve states.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
