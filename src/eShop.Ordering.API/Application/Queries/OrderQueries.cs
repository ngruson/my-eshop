using eShop.Ordering.API.Application.Specifications;
using eShop.Shared.Data;
using BuyerAggregate = eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;
using OrderAggregate = eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace eShop.Ordering.API.Application.Queries;

public class OrderQueries(
    IRepository<CardType> cardTypeRepository,
    IRepository<OrderAggregate.Order> orderRepository)
    : IOrderQueries
{
    private readonly IRepository<OrderAggregate.Order> orderRepository = orderRepository;
    private readonly IRepository<CardType> cardTypeRepository = cardTypeRepository;

    public async Task<Order> GetOrderAsync(int id)
    {
        OrderAggregate.Order order = await this.orderRepository.SingleOrDefaultAsync(new GetOrderSpecification(id))
            ?? throw new KeyNotFoundException();

        return new Order
        {
            OrderNumber = order.Id,
            Date = order.OrderDate,
            Description = order.Description!,
            City = order.Address!.City,
            Country = order.Address.Country,
            State = order.Address.State,
            Street = order.Address.Street,
            ZipCode = order.Address.ZipCode,
            Status = order.OrderStatus.ToString(),
            Total = order.GetTotal(),
            OrderItems = order.OrderItems.Select(oi => new OrderItem
            {
                ProductName = oi.ProductName!,
                Units = oi.Units,
                UnitPrice = (double)oi.UnitPrice,
                PictureUrl = oi.PictureUrl!
            }).ToList()
        };
    }

    public async Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(string userId)
    {
        List<OrderAggregate.Order> orders = await this.orderRepository.ListAsync(new GetOrdersFromUserSpecification(userId));

        return orders
            .Select(o => new OrderSummary
            {
                OrderNumber = o.Id,
                Date = o.OrderDate,
                Status = o.OrderStatus.ToString(),
                Total = (double)o.OrderItems.Sum(oi => oi.UnitPrice * oi.Units)
            });
    }

    public async Task<IEnumerable<CardType>> GetCardTypesAsync()
    {
        return await this.cardTypeRepository.ListAsync();
    }        
}
