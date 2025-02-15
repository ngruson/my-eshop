using Ardalis.Result;
using eShop.MasterData.Contracts;
using eShop.ServiceInvocation.MasterDataApiClient;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace eShop.AdminApp.Application.Queries.MasterData.GetCountries;

public class GetCountriesQueryHandler(
    ILogger<GetCountriesQueryHandler> logger,
    IMasterDataApiClient masterDataApiClient,
    IMemoryCache cache) : IRequestHandler<GetCountriesQuery, Result<CountryViewModel[]>>
{
    private readonly ILogger<GetCountriesQueryHandler> logger = logger;
    private readonly IMasterDataApiClient masterDataApiClient = masterDataApiClient;
    private readonly IMemoryCache cache = cache;

    public async Task<Result<CountryViewModel[]>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Retrieving countries from cache or master data API.");

            CountryViewModel[] viewModel;
            string key = "Countries";
            if (!this.cache.TryGetValue(key, out object? countries))
            {
                this.logger.LogInformation("Countries not found in cache. Retrieving from master data API.");
                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1));
                CountryDto[] dto = await this.masterDataApiClient.GetCountries();
                viewModel = [.. dto.MapToCountryViewModels()];
                this.cache.Set(key, viewModel, cacheEntryOptions);
            }
            else
            {
                viewModel = (CountryViewModel[])countries!;
            }
            
            this.logger.LogInformation("Countries retrieved successfully.");

            return viewModel;
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve countries.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
