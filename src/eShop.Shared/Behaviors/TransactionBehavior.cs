namespace eShop.Shared.Behaviors;

using eShop.EventBus.Extensions;
using eShop.Shared.Data.EntityFramework;
using eShop.Shared.IntegrationEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

public class TransactionBehavior<TRequest, TResponse>(eShopDbContext dbContext,
    IIntegrationEventService _integrationEventService,
    ILogger<TransactionBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse?> where TRequest : IRequest<TResponse?>
{
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger = logger ?? throw new ArgumentException(nameof(ILogger));
    private readonly eShopDbContext _dbContext = dbContext ?? throw new ArgumentException(nameof(eShopDbContext));
    private readonly IIntegrationEventService _integrationEventService = _integrationEventService ?? throw new ArgumentException(null, nameof(_integrationEventService));

    public async Task<TResponse?> Handle(TRequest request, RequestHandlerDelegate<TResponse?> next, CancellationToken cancellationToken)
    {
        TResponse? response = default;
        var typeName = request.GetGenericTypeName();

        try
        {
            if (this._dbContext.HasActiveTransaction)
            {
                return await next();
            }

            IExecutionStrategy strategy = this._dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                Guid transactionId;

                await using IDbContextTransaction? transaction = await this._dbContext.BeginTransactionAsync();

                if (transaction is not null)
                {
                    using (this._logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction.TransactionId) }))
                    {
                        this._logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                        response = await next();

                        this._logger.LogInformation("Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                        await this._dbContext.CommitTransactionAsync(transaction);

                        transactionId = transaction.TransactionId;
                    }

                    await this._integrationEventService.PublishEventsThroughEventBusAsync(transactionId, cancellationToken);
                }
            });

            return response;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Error Handling transaction for {CommandName} ({@Command})", typeName, request);

            throw;
        }
    }
}
