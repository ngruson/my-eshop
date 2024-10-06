using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.Customer.GetCustomers;
using eShop.AdminApp.Application.Queries.MasterData.GetCountries;
using eShop.AdminApp.Application.Queries.MasterData.GetStates;
using eShop.Customer.Contracts;
using eShop.Customer.Contracts.GetCustomers;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetCustomersQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCustomerCreated(
        GetCustomersQuery query,
        [Substitute, Frozen] ICustomerApi customerApi,
        [Substitute, Frozen] IMediator mediator,
        GetCustomersQueryHandler sut,
        CustomerDto[] customers,
        CountryViewModel[] countries,
        StateViewModel[] states)
    {
        // Arrange

        customerApi.GetCustomers()
            .Returns(customers);

        mediator.Send(Arg.Any<GetCountriesQuery>(), Arg.Any<CancellationToken>())
            .Returns(countries);
        mediator.Send(Arg.Any<GetStatesQuery>(), Arg.Any<CancellationToken>())
            .Returns(states);

        // Act

        Result<List<CustomerViewModel>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await customerApi.Received().GetCustomers();
        await mediator.Received().Send(Arg.Any<GetCountriesQuery>(), Arg.Any<CancellationToken>());
        await mediator.Received().Send(Arg.Any<GetStatesQuery>(), Arg.Any<CancellationToken>());
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCustomersQuery query,
        [Substitute, Frozen] ICustomerApi customerApi,
        [Substitute, Frozen] IMediator mediator,
        GetCustomersQueryHandler sut)
    {
        // Arrange

        customerApi.GetCustomers()
            .ThrowsAsync<Exception>();

        // Act

        Result<List<CustomerViewModel>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await customerApi.Received().GetCustomers();
        await mediator.DidNotReceive().Send(Arg.Any<GetCountriesQuery>(), Arg.Any<CancellationToken>());
        await mediator.DidNotReceive().Send(Arg.Any<GetStatesQuery>(), Arg.Any<CancellationToken>());
    }
}
