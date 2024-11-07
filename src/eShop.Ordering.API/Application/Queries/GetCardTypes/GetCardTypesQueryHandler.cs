using Ardalis.Result;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.Shared.Data;

namespace eShop.Ordering.API.Application.Queries.GetCardTypes;

internal class GetCardTypesQueryHandler(
    ILogger<GetCardTypesQueryHandler> logger,
    IRepository<CardType> repository) : IRequestHandler<GetCardTypesQuery, Result<CardTypeDto[]>>
{
    private readonly ILogger<GetCardTypesQueryHandler> logger = logger;
    private readonly IRepository<CardType> cardTypeRepository = repository;

    public async Task<Result<CardTypeDto[]>> Handle(GetCardTypesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.LogInformation("Retrieving card types");

            List<CardType> cardTypes = await this.cardTypeRepository.ListAsync();            

            this.logger.LogInformation("Card types retrieved");

            return cardTypes.Select(ct => new CardTypeDto(ct.ObjectId, ct.Name!)).ToArray();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to retrieve card types.";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }
}
