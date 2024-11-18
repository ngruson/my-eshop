using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Customer.Contracts.CreateCustomer;
using eShop.Customer.Contracts.UpdateCustomerGeneralInfo;
using eShop.Customer.Contracts;
using NSubstitute;

namespace eShop.ServiceInvocation.UnitTests.Refit;

public class CustomerApiClientUnitTests
{
    public class CreateCustomer
    {
        [Theory, AutoNSubstituteData]
        public async Task create_customer(
            [Substitute, Frozen] ICustomerApi customerApi,
            CustomerApiClient.Refit.CustomerApiClient sut,
            CreateCustomerDto dto)
        {
            // Arrange            

            // Act

            await sut.CreateCustomer(dto);

            // Assert

            await customerApi.Received().CreateCustomer(dto);
        }
    }

    public class DeleteCustomer
    {
        [Theory, AutoNSubstituteData]
        public async Task delete_customer(
            [Substitute, Frozen] ICustomerApi customerApi,
            CustomerApiClient.Refit.CustomerApiClient sut,
            Guid objectId)
        {
            // Arrange

            // Act

            await sut.DeleteCustomer(objectId);

            // Assert

            await customerApi.Received().DeleteCustomer(objectId);
        }
    }

    public class GetCustomer
    {
        [Theory, AutoNSubstituteData]
        public async Task return_customer(
            [Substitute, Frozen] ICustomerApi customerApi,
            CustomerApiClient.Refit.CustomerApiClient sut,
            Guid objectId,
            Customer.Contracts.GetCustomer.CustomerDto customer)
        {
            // Arrange
            customerApi.GetCustomer(objectId)
                .Returns(customer);

            // Act

            Customer.Contracts.GetCustomer.CustomerDto actual = await sut.GetCustomer(objectId);

            // Assert

            Assert.Equivalent(actual, customer);
        }
    }

    public class GetCustomerByName
    {
        [Theory, AutoNSubstituteData]
        public async Task return_customer(
            [Substitute, Frozen] ICustomerApi customerApi,
            CustomerApiClient.Refit.CustomerApiClient sut,
            string name,
            Customer.Contracts.GetCustomer.CustomerDto customer)
        {
            // Arrange

            customerApi.GetCustomer(name)
                .Returns(customer);

            // Act

            Customer.Contracts.GetCustomer.CustomerDto actual = await sut.GetCustomer(name);

            // Assert

            Assert.Equivalent(actual, customer);
        }
    }

    public class GetCustomers
    {
        [Theory, AutoNSubstituteData]
        public async Task return_customers(
            [Substitute, Frozen] ICustomerApi customerApi,
            CustomerApiClient.Refit.CustomerApiClient sut,
            Customer.Contracts.GetCustomers.CustomerDto[] customers,
            bool includeDeleted)
        {
            // Arrange

            customerApi.GetCustomers(includeDeleted)
                .Returns(customers);

            // Act

            Customer.Contracts.GetCustomers.CustomerDto[] actual = await sut.GetCustomers(includeDeleted);

            // Assert

            Assert.Equivalent(actual, customers);
        }
    }

    public class UpdateCustomer
    {
        [Theory, AutoNSubstituteData]
        public async Task update_customer(
            [Substitute, Frozen] ICustomerApi customerApi,
            CustomerApiClient.Refit.CustomerApiClient sut,
            Guid objectId,
            UpdateCustomerDto dto)
        {
            // Arrange

            // Act

            await sut.UpdateCustomer(objectId, dto);

            // Assert

            await customerApi.Received().UpdateCustomer(objectId, dto);
        }
    }
}
