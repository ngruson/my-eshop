namespace eShop.Ordering.API.Application.Commands;

/// <summary>
/// Provides a base implementation for handling duplicate request and ensuring idempotent updates, in the cases where
/// a request id sent by client is used to detect duplicate requests.
/// </summary>
/// <typeparam name="T">Type of the command handler that performs the operation if request is not duplicated</typeparam>
/// <typeparam name="R">Return value of the inner command handler</typeparam>
public abstract class IdentifiedCommandHandler<T, R>(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<T, R>> logger) : IRequestHandler<IdentifiedCommand<T, R>, R?>
    where T : IRequest<R>
{
    /// <summary>
    /// Creates the result value to return if a previous request was found
    /// </summary>
    /// <returns></returns>
    protected abstract R CreateResultForDuplicateRequest();

    /// <summary>
    /// This method handles the command. It just ensures that no other request exists with the same ID, and if this is the case
    /// just enqueues the original inner command.
    /// </summary>
    /// <param name="message">IdentifiedCommand which contains both original command & request ID</param>
    /// <returns>Return value of inner command or default value if request same ID was found</returns>
    public async Task<R?> Handle(IdentifiedCommand<T, R> message, CancellationToken cancellationToken)
    {
        bool alreadyExists = await requestManager.ExistAsync(message.Id);
        if (alreadyExists)
        {
            return this.CreateResultForDuplicateRequest();
        }
        else
        {
            await requestManager.CreateRequestForCommandAsync<T>(message.Id);
            try
            {
                T command = message.Command;
                string commandName = command.GetGenericTypeName();
                string idProperty = string.Empty;
                string commandId = string.Empty;

                switch (command)
                {
                    case CreateOrderCommand createOrderCommand:
                        idProperty = nameof(createOrderCommand.UserId);
                        commandId = createOrderCommand.UserId;
                        break;

                    case CancelOrderCommand cancelOrderCommand:
                        idProperty = nameof(cancelOrderCommand.OrderNumber);
                        commandId = $"{cancelOrderCommand.OrderNumber}";
                        break;

                    case ShipOrderCommand shipOrderCommand:
                        idProperty = nameof(shipOrderCommand.OrderNumber);
                        commandId = $"{shipOrderCommand.OrderNumber}";
                        break;

                    default:
                        idProperty = "Id?";
                        commandId = "n/a";
                        break;
                }

                logger.LogInformation(
                    "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    commandName,
                    idProperty,
                    commandId,
                    command);

                // Send the embedded business command to mediator so it runs its related CommandHandler 
                R? result = await mediator.Send(command, cancellationToken);

                logger.LogInformation(
                    "Command result: {@Result} - {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    result,
                    commandName,
                    idProperty,
                    commandId,
                    command);

                return result;
            }
            catch
            {
                return default;
            }
        }
    }
}
