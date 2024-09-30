using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Ordering.Contracts;
using NSubstitute;
using eShop.AdminApp.Application.Commands.GenerateOrders;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Customer.Contracts;
using eShop.Customer.Contracts.GetCustomers;
using eShop.Identity.Contracts;
using eShop.Catalog.Contracts;
using eShop.Identity.Contracts.GetUsers;
using eShop.Catalog.Contracts.GetCatalogItems;

namespace eShop.AdminApp.UnitTests.Application.Commands;

public class GenerateOrdersCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenOrdersCreated(
        GenerateOrdersCommand command,
        [Substitute, Frozen] ICustomerApi customerApi,
        [Substitute, Frozen] IIdentityApi identityApi,
        [Substitute, Frozen] ICatalogApi catalogApi,
        [Substitute, Frozen] IOrderingApi orderingApi,
        GenerateOrdersCommandHandler sut,
        CustomerDto[] customers,
        UserDto[] users,
        GetCatalogItemsResponse getCatalogItemsResponse)
    {
        // Arrange

        customerApi.GetCustomers()
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

        identityApi.GetUsers()
            .Returns(users2);

        catalogApi.GetCatalogItems()
            .Returns(getCatalogItemsResponse);

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await orderingApi.Received(command.OrdersToCreate).CreateOrder(Arg.Any<Guid>(), Arg.Any<CreateOrderDto>());
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GenerateOrdersCommand command,
        [Substitute, Frozen] IOrderingApi orderingApi,
        GenerateOrdersCommandHandler sut)
    {
        // Arrange

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await orderingApi.DidNotReceive().CreateOrder(Arg.Any<Guid>(), Arg.Any<CreateOrderDto>());
    }
}
