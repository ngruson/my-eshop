using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.AdminApp.Application.Queries.Customer.GetCustomers;
using eShop.AdminApp.Application.Queries.MasterData.GetCountries;
using eShop.AdminApp.Application.Queries.MasterData.GetStates;
using eShop.Customer.Contracts.GetCustomers;
using eShop.ServiceInvocation.CustomerService;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace eShop.AdminApp.UnitTests.Application.Queries;

public class GetCustomersQueryUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task ReturnSuccessWhenCustomerCreated(
        GetCustomersQuery query,
        [Substitute, Frozen] ICustomerService customerService,
        [Substitute, Frozen] IMediator mediator,
        GetCustomersQueryHandler sut,
        CustomerDto[] customers,
        CountryViewModel[] countries,
        StateViewModel[] states)
    {
        // Arrange

        customerService.GetCustomers()
            .Returns(customers);

        mediator.Send(Arg.Any<GetCountriesQuery>(), Arg.Any<CancellationToken>())
            .Returns(countries);
        mediator.Send(Arg.Any<GetStatesQuery>(), Arg.Any<CancellationToken>())
            .Returns(states);

        // Act

        Result<List<CustomerViewModel>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await customerService.Received().GetCustomers();
        await mediator.Received().Send(Arg.Any<GetCountriesQuery>(), Arg.Any<CancellationToken>());
        await mediator.Received().Send(Arg.Any<GetStatesQuery>(), Arg.Any<CancellationToken>());
    }

    [Theory, AutoNSubstituteData]
    internal async Task ReturnErrorWhenExceptionIsThrown(
        GetCustomersQuery query,
        [Substitute, Frozen] ICustomerService customerService,
        [Substitute, Frozen] IMediator mediator,
        GetCustomersQueryHandler sut)
    {
        // Arrange

        customerService.GetCustomers()
            .ThrowsAsync<Exception>();

        // Act

        Result<List<CustomerViewModel>> result = await sut.Handle(query, CancellationToken.None);

        // Assert

        Assert.True(result.IsError());

        await customerService.Received().GetCustomers();
        await mediator.DidNotReceive().Send(Arg.Any<GetCountriesQuery>(), Arg.Any<CancellationToken>());
        await mediator.DidNotReceive().Send(Arg.Any<GetStatesQuery>(), Arg.Any<CancellationToken>());
    }
}
