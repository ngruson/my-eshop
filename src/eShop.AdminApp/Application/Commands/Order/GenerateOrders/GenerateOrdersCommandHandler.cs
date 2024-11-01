using System.Text.RegularExpressions;
using Ardalis.Result;
using eShop.AdminApp.Application.Commands.Order.GenerateOrders;
using eShop.Catalog.Contracts.GetCatalogItems;
using eShop.Customer.Contracts.GetCustomers;
using eShop.Identity.Contracts;
using eShop.Identity.Contracts.GetUsers;
using eShop.Ordering.Contracts.CreateOrder;
using eShop.Ordering.Contracts.GetCardTypes;
using eShop.ServiceInvocation.CatalogApiClient;
using eShop.ServiceInvocation.CustomerApiClient;
using eShop.ServiceInvocation.IdentityApiClient;
using eShop.ServiceInvocation.OrderingApiClient;
using MediatR;

namespace eShop.AdminApp.Application.Commands.GenerateOrders;

internal partial class GenerateOrdersCommandHandler(
    ILogger<GenerateOrdersCommandHandler> logger,
    IConfiguration config,
    ICustomerApiClient customerApiClient,
    IIdentityApiClient identityService,
    ICatalogApiClient catalogApiClient,
    IOrderingApiClient orderingApiClient) : IRequestHandler<GenerateOrdersCommand, Result>
{
    private readonly ILogger<GenerateOrdersCommandHandler> logger = logger;
    private readonly ICustomerApiClient customerApiClient = customerApiClient;
    private readonly IIdentityApiClient identityService = identityService;
    private readonly ICatalogApiClient catalogApiClient = catalogApiClient;
    private readonly IOrderingApiClient orderingApiClient = orderingApiClient;

    private readonly string catalogApiUrl = config["services:catalog-api:http:0"]!;

    public async Task<Result> Handle(GenerateOrdersCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all customers

            this.WriteProgress(request, (0, "Getting customers..."));
            CustomerDto[] customers = await this.customerApiClient.GetCustomers();
            this.WriteProgress(request, (0, "Received {Count} customers"), customers.Length);

            // Get all users

            this.WriteProgress(request, (0, "Getting users..."));
            UserDto[] users = await this.identityService.GetUsers();
            this.WriteProgress(request, (0, "Received {Count} users"), users.Length);

            // Get all products
            this.WriteProgress(request, (0, "Getting catalog items..."));
            CatalogItemDto[] catalogItems = await this.catalogApiClient.GetCatalogItems();
            this.WriteProgress(request, (0, "Received {Count} catalog items"), catalogItems.Length);

            IEnumerable<CardTypeDto> cardTypes = await this.orderingApiClient.GetCardTypes();
            CardTypeDto cardType = cardTypes.First(_ => _.Name == "Amex");

            Random random = new();

            for (int i = 0; i < request.OrdersToCreate; i++)
            {
                CustomerDto customer = customers[random.Next(customers.Length)];
                UserDto user = users.Single(_ => _.UserName == customer.UserName);

                int orderItemsCount = random.Next(0, 5);
                OrderItemDto[] orderItems = new OrderItemDto[orderItemsCount];

                for (int j = 0; j < orderItemsCount; j++)
                {
                    CatalogItemDto catalogItem = catalogItems[random.Next(catalogItems.Length)];

                    orderItems[j] = new(
                        catalogItem.ObjectId,
                        catalogItem.Name,
                        catalogItem.Price,
                        0,
                        random.Next(1, 6),
                        $"{this.catalogApiUrl}/api/catalog/items/{catalogItem.ObjectId}/pic");
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
                    cardType.ObjectId,
                    user.Id,
                    orderItems);

                this.WriteProgress(request, (i, "Creating order {Counter}..."), i);

                await this.orderingApiClient.CreateOrder(Guid.NewGuid(), order);

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

        if (items.Count > 0)
        {
            Dictionary<string, object> keyValuePairs = [];
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
        List<string> items = [];
        MatchCollection matches = AccoladesRegex().Matches(input);

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

    [GeneratedRegex(@"\{(.*?)\}")]
    private static partial Regex AccoladesRegex();
}
