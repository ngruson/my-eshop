using CsvHelper;
using eShop.Identity.API.Api.Commands.CreateUser;
using eShop.Identity.Contracts.CreateUser;
using eShop.Shared.Data.Seed;
using eShop.Shared.DI;
using MediatR;
using System.Globalization;

namespace eShop.Identity.API.Seed;

public class CustomersSeed(IMediator mediator) : IDbSeeder
{
    private readonly IMediator mediator = mediator;

    public async Task SeedAsync(ServiceProviderWrapper serviceProviderWrapper)
    {
        using StreamReader reader = new("seed//customers.csv");
        using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
        List<CustomerCsv> records = csv.GetRecords<CustomerCsv>().Take(10).ToList();

        foreach (var customer in records)
        {
            CreateUserDto createUserDto = new(
                customer.UserName!,
                customer.Email!,
                customer.PhoneNumber!
            );
            await this.mediator.Send(new CreateUserCommand(createUserDto));
        }
    }
}
