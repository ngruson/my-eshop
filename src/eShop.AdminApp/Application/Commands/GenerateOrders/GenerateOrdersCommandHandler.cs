using System.Text.RegularExpressions;
using Ardalis.Result;
using eShop.Catalog.Contracts;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Customer.Contracts;
using eShop.Customer.Contracts.GetCustomers;
using eShop.Identity.Contracts;
using eShop.Identity.Contracts.GetUsers;
using eShop.Ordering.Contracts;
using eShop.Ordering.Contracts.CreateOrder;
using MediatR;

namespace eShop.AdminApp.Application.Commands.GenerateOrders;

internal class GenerateOrdersCommandHandler(
    ILogger<GenerateOrdersCommandHandler> logger,
    IConfiguration config,
    ICustomerApi customerApi,
    IIdentityApi identityApi,
    ICatalogApi catalogApi,
    IOrderingApi orderingApi) : IRequestHandler<GenerateOrdersCommand, Result>
{
    private readonly ILogger<GenerateOrdersCommandHandler> logger = logger;
    private readonly ICustomerApi customerApi = customerApi;
    private readonly IIdentityApi identityApi = identityApi;
    private readonly ICatalogApi catalogApi = catalogApi;
    private readonly IOrderingApi orderingApi = orderingApi;

    private readonly string catalogApiUrl = config["services:catalog-api:http:0"]!;

    public async Task<Result> Handle(GenerateOrdersCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all customers

            this.WriteProgress(request, (0, "Getting customers..."));
            CustomerDto[] customers = await this.customerApi.GetCustomers();
            this.WriteProgress(request, (0, "Received {Count} customers"), customers.Length);

            // Get all users

            this.WriteProgress(request, (0, "Getting users..."));
            UserDto[] users = await this.identityApi.GetUsers();
            this.WriteProgress(request, (0, "Received {Count} users"), users.Length);

            // Get all products
            this.WriteProgress(request, (0, "Getting catalog items..."));
            GetCatalogItemsResponse catalogItemsResponse = await this.catalogApi.GetCatalogItems();
            CatalogItemDto[] catalogItems = catalogItemsResponse.Data;
            this.WriteProgress(request, (0, "Received {Count} catalog items"), catalogItems.Length);

            Random random = new();

            for (int i = 0; i < request.OrdersToCreate; i++)
            {
                CustomerDto customer = customers[random.Next(customers.Length)];
                UserDto user = users.Single(_ => _.UserName == customer.UserName);

                OrderItemDto[] orderItems = new OrderItemDto[4];

                for (int j = 0; j < random.Next(0, 5); j++)
                {
                    CatalogItemDto catalogItem = catalogItems[random.Next(catalogItems.Length)];

                    orderItems[j] = new(
                        catalogItem.Id,
                        catalogItem.Name,
                        catalogItem.Price,
                        0,
                        random.Next(1, 6),
                        $"{this.catalogApiUrl}/api/catalog/items/{catalogItem.Id}/pic");
                }

                CreateOrderDto order = new(
                    UserId: user.Id,
                    UserName: user.UserName,
                    City: customer.City!,
                    Street: customer.Street!,
                    State: customer.State!,
                    Country: customer.Country!,
                    ZipCode: customer.ZipCode!,
                    CardNumber: "1111222233334444",
                    CardHolderName: "TESTUSER",
                    CardExpiration: DateTime.UtcNow.AddYears(1),
                    CardSecurityNumber: "111",
                    1,
                    user.Id,
                    orderItems);

                this.WriteProgress(request, (i, "Creating order {Counter}..."), i);

                await this.orderingApi.CreateOrder(Guid.NewGuid(), order);

                this.WriteProgress(request, (i, "Order {Counter} created"), i);
            }

            this.WriteProgress(request, (request.OrdersToCreate, "{Count} orders created"), request.OrdersToCreate);

            return Result.Success();
        }
        catch (Exception ex)
        {
            string errorMessage = "Failed to generate orders";
            this.logger.LogError(ex, "Error: {Message}", errorMessage);
            return Result.Error(errorMessage);
        }
    }

    private void WriteProgress(GenerateOrdersCommand request, (int, string) progress, params object[] args)
    {
        this.logger.LogInformation(progress.Item2, args);

        string progressMessage;
        List<string> items = ExtractItems(progress.Item2);

        if (items.Any())
        {
            Dictionary<string, object> keyValuePairs = new();
            foreach (string item in items)
            {
                keyValuePairs.Add(item, args[items.IndexOf(item)]);
            }

            progressMessage = ReplacePlaceholders(progress.Item2, keyValuePairs);
        }
        else
        {
            progressMessage = progress.Item2;
        }

        request.ProgressService.ReportProgress((progress.Item1, progressMessage));
    }

    private static List<string> ExtractItems(string input)
    {
        List<string> items = new List<string>();
        Regex regex = new Regex(@"\{(.*?)\}");
        MatchCollection matches = regex.Matches(input);

        foreach (Match match in matches)
        {
            items.Add(match.Groups[1].Value);
        }

        return items;
    }

    private static string ReplacePlaceholders(string template, Dictionary<string, object> parameters)
    {
        foreach (var parameter in parameters)
        {
            template = template.Replace($"{{{parameter.Key}}}", parameter.Value.ToString());
        }
        return template;
    }
}
