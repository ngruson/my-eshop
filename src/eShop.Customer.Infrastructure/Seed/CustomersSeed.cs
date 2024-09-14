using CsvHelper;
using Duende.AccessTokenManagement;
using eShop.Identity.Contracts;
using eShop.Identity.Contracts.CreateUser;
using eShop.Shared.Data;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace eShop.Customer.Infrastructure.Seed;

public class CustomersSeed(
    IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository,
    IIdentityApi identityApi,
    IClientCredentialsTokenManagementService clientCredentialsTokenManagement,
    IConfiguration configuration)
{
    private readonly IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository = customerRepository;
    private readonly IIdentityApi identityApi = identityApi;
    private readonly IClientCredentialsTokenManagementService clientCredentialsTokenManagement = clientCredentialsTokenManagement;

    public async Task SeedAsync()
    {
        if (!await this.customerRepository.AnyAsync())
        {
            using StreamReader reader = new("seed\\customers.csv");
            using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
            List<CustomerCsv> records = csv.GetRecords<CustomerCsv>().Take(100).ToList();

            List<Domain.AggregatesModel.CustomerAggregate.Customer> customers =
            records.Select(r => new Domain.AggregatesModel.CustomerAggregate.Customer(
                r.FirstName,
                r.LastName,
                r.CardNumber,
                "123",
                "12/24",
                r.CardHolderName,
                1,
                r.Street,
                r.City,
                r.State,
                r.Country,
                r.ZipCode
            )).ToList();

            await this.customerRepository.AddRangeAsync(customers);

            ClientCredentialsToken clientCredentialsToken = await this.clientCredentialsTokenManagement.GetAccessTokenAsync(
                configuration["Identity:ClientCredentials:ClientId"]!);

            foreach (var customer in records)
            {
                CreateUserDto createUserDto = new(
                    customer.UserName!,
                    customer.Email!,
                    customer.PhoneNumber!
                );
                await this.identityApi.CreateUser(clientCredentialsToken.AccessToken!, createUserDto);
            }
        }
    }
}
