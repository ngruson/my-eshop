using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using eShop.Customer.Contracts.CreateCustomer;
using eShop.Customer.Contracts.UpdateCustomerGeneralInfo;
using NSubstitute;
using Dapr.Client;
using eShop.ServiceInvocation.Auth;

namespace eShop.ServiceInvocation.UnitTests.Dapr;

public class CustomerApiClientUnitTests
{
    public class CreateCustomer
    {
        [Theory, AutoNSubstituteData]
        public async Task create_customer(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CustomerApiClient.Dapr.CustomerApiClient sut,
            CreateCustomerDto dto,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Post,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>(),
                dto)
            .Returns(httpRequestMessage);

            // Act

            await sut.CreateCustomer(dto);

            // Assert

            await daprClient.Received().InvokeMethodAsync(httpRequestMessage);
        }
    }

    public class DeleteCustomer
    {
        [Theory, AutoNSubstituteData]
        public async Task delete_customer(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CustomerApiClient.Dapr.CustomerApiClient sut,
            Guid objectId,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Delete,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
                    .Returns(httpRequestMessage);

            // Act

            await sut.DeleteCustomer(objectId);

            // Assert

            await daprClient.Received().InvokeMethodAsync(httpRequestMessage);
        }
    }

    public class GetCustomer
    {
        [Theory, AutoNSubstituteData]
        public async Task return_customer(
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CustomerApiClient.Dapr.CustomerApiClient sut,
            Guid objectId,
            Customer.Contracts.GetCustomer.CustomerDto customer,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
                    .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<Customer.Contracts.GetCustomer.CustomerDto>(httpRequestMessage)
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
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CustomerApiClient.Dapr.CustomerApiClient sut,
            string name,
            Customer.Contracts.GetCustomer.CustomerDto customer,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
                    .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<Customer.Contracts.GetCustomer.CustomerDto>(httpRequestMessage)
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
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CustomerApiClient.Dapr.CustomerApiClient sut,
            Customer.Contracts.GetCustomers.CustomerDto[] customers,
            bool includeDeleted,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Get,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>())
                    .Returns(httpRequestMessage);

            daprClient.InvokeMethodAsync<Customer.Contracts.GetCustomers.CustomerDto[]>(httpRequestMessage)
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
            [Substitute, Frozen] IAccessTokenAccessor accessTokenAccessor,
            [Substitute, Frozen] AccessTokenAccessorFactory accessTokenAccessorFactory,
            [Substitute, Frozen] DaprClient daprClient,
            CustomerApiClient.Dapr.CustomerApiClient sut,
            Guid objectId,
            UpdateCustomerDto dto,
            HttpRequestMessage httpRequestMessage,
            string accessToken)
        {
            // Arrange

            accessTokenAccessor.GetAccessToken().Returns(accessToken);
            accessTokenAccessorFactory.Create().Returns(accessTokenAccessor);

            daprClient.CreateInvokeMethodRequest(
                HttpMethod.Put,
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IReadOnlyCollection<KeyValuePair<string, string>>>(),
                dto)
            .Returns(httpRequestMessage);

            // Act

            await sut.UpdateCustomer(objectId, dto);

            // Assert

            await daprClient.Received().InvokeMethodAsync(httpRequestMessage);
        }
    }
}
