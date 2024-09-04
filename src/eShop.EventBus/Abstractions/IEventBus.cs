﻿using Ardalis.Result;

namespace eShop.EventBus.Abstractions;

public interface IEventBus
{
    Task<Result> PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken);
}