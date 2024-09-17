using Ardalis.Result;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Customer.API.Application.Commands.DeleteCustomer;
using eShop.Customer.API.Application.Specifications;
using eShop.Shared.Data;
using NSubstitute;

namespace eShop.Customer.UnitTests.Application.Commands;

public class DeleteCustomerCommandUnitTests
{
    [Theory, AutoNSubstituteData]
    internal async Task Handle_ShouldReturnSuccessGivenCustomerDeleted(
        DeleteCustomerCommand command,
        [Substitute, Frozen] IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository,
        DeleteCustomerCommandHandler sut,
        Domain.AggregatesModel.CustomerAggregate.Customer customer)
    {
        // Arrange

        customerRepository.FirstOrDefaultAsync(
                Arg.Any<GetCustomerSpecification>(),
                default)
            .Returns(customer);

        // Act

        Result result = await sut.Handle(command, CancellationToken.None);

        // Assert

        Assert.True(result.IsSuccess);

        await customerRepository.Received().DeleteAsync(Arg.Any<Domain.AggregatesModel.CustomerAggregate.Customer>(), default);
    }
}
