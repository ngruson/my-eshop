using Ardalis.Result;
using eShop.Basket.API.Model;

namespace eShop.Basket.API.Repositories;

public interface IBasketRepository
{
    Task<Result<CustomerBasket>> GetBasketAsync(string customerId);
    Task<Result<CustomerBasket>> UpdateBasketAsync(CustomerBasket basket);
    Task<Result> DeleteBasketAsync(string id);
}
