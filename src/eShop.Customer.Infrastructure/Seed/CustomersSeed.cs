using CsvHelper;
using eShop.Shared.Data;
using eShop.Shared.Data.Seed;
using eShop.Shared.DI;
using System.Globalization;

namespace eShop.Customer.Infrastructure.Seed;

public class CustomersSeed(IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository) : IDbSeeder
{
    private readonly IRepository<Domain.AggregatesModel.CustomerAggregate.Customer> customerRepository = customerRepository;    

    public async Task SeedAsync(ServiceProviderWrapper serviceProviderWrapper)
    {
        if (!await this.customerRepository.AnyAsync())
        {
            using StreamReader reader = new($"{AppContext.BaseDirectory}//seed//customers.csv");
            using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
            List<CustomerCsv> records = csv.GetRecords<CustomerCsv>().Take(10).ToList();

            List<Domain.AggregatesModel.CustomerAggregate.Customer> customers =
                records.Select(r => new Domain.AggregatesModel.CustomerAggregate.Customer(
                    r.UserName,
                    r.FirstName,
                    r.LastName,
                    r.Street,
                    r.City,
                    r.State,
                    r.Country,
                    r.ZipCode,
                    r.CardNumber,
                    "123",
                    "12/24",
                    r.CardHolderName,
                    1)).ToList();

            await this.customerRepository.AddRangeAsync(customers);
        }
    }
}
