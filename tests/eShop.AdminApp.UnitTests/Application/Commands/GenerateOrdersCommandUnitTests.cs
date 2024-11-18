using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using NSubstitute;
using eShop.AdminApp.Application.Commands.GenerateOrders;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Customer.Contracts.GetCustomers;
using eShop.Identity.Contracts.GetUsers;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.AdminApp.Application.Commands.Order.GenerateOrders;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.ServiceInvocation.CustomerApiClient;
using eShop.ServiceInvocation.CatalogApiClient;
using eShop.ServiceInvocation.IdentityApiClient;
using eShop.ServiceInvocation.OrderingApiClient;

namespace eShop.AdminApp.UnitTests.Application.Commands;

public class GenerateOrdersCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenOrdersCreated(
        GenerateOrdersCommand command,
        [Substitute, Frozen] ICustomerApiClient customerApiClient,
        [Substitute, Frozen] IIdentityApiClient identityService,
        [Substitute, Frozen] ICatalogApiClient catalogApiClient,
        [Substitute, Frozen] IOrderingApiClient orderingApiClient,
        GenerateOrdersCommandHandler sut,
        CustomerDto[] customers,
        UserDto[] users,
        CatalogItemDto[] catalogItems,
        CardTypeDto[] cardTypes)
    {
        // Arrange

        customerApiClient.GetCustomers()
            .Returns(customers);

        UserDto[] users2 = new UserDto[users.Length];

        for (int i = 0; i < users.Length; i++)
        {
            users2[i] = new UserDto(
                users[i].Id,
                customers[i].UserName,
                users[i].FirstName,
                users[i].LastName);
        }

        identityService.GetUsers()
            .Returns(users2);

        catalogApiClient.GetCatalogItems()
            .Returns(catalogItems);

        cardTypes[0] = cardTypes[0] with { Name = "Amex" };

        orderingApiClient.GetCardTypes()
            .Returns(cardTypes);

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await orderingApiClient.Received(command.OrdersToCreate).CreateOrder(Arg.Any<Guid>(), Arg.Any<OrderDto>());
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GenerateOrdersCommand command,
        [Substitute, Frozen] IOrderingApiClient orderingApiClient,
        GenerateOrdersCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await orderingApiClient.DidNotReceive().CreateOrder(Arg.Any<Guid>(), Arg.Any<OrderDto>());
    }
}
