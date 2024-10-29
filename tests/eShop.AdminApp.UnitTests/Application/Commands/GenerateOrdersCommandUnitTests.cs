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
using eShop.ServiceInvocation.OrderingService;
using eShop.ServiceInvocation.CatalogService;
using eShop.ServiceInvocation.CustomerService;
using eShop.ServiceInvocation.IdentityService;

namespace eShop.AdminApp.UnitTests.Application.Commands;

public class GenerateOrdersCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenOrdersCreated(
        GenerateOrdersCommand command,
        [Substitute, Frozen] ICustomerService customerService,
        [Substitute, Frozen] IIdentityService identityService,
        [Substitute, Frozen] ICatalogService catalogService,
        [Substitute, Frozen] IOrderingService orderingService,
        GenerateOrdersCommandHandler sut,
        CustomerDto[] customers,
        UserDto[] users,
        CatalogItemDto[] catalogItems,
        CardTypeDto[] cardTypes)
    {
        // Arrange

        customerService.GetCustomers()
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

        catalogService.GetCatalogItems()
            .Returns(catalogItems);

        cardTypes[0] = cardTypes[0] with { Name = "Amex" };

        orderingService.GetCardTypes()
            .Returns(cardTypes);

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await orderingService.Received(command.OrdersToCreate).CreateOrder(Arg.Any<Guid>(), Arg.Any<CreateOrderDto>());
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GenerateOrdersCommand command,
        [Substitute, Frozen] IOrderingService orderingService,
        GenerateOrdersCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await orderingService.DidNotReceive().CreateOrder(Arg.Any<Guid>(), Arg.Any<CreateOrderDto>());
    }
}
