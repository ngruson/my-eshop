namespace eShop.ClientApp.UnitTests.Services;

[TestClass]
public class OrdersServiceTests
{
    [TestMethod]
    public async Task GetFakeOrderTest()
    {
        var ordersMockService = new OrderMockService();
        var order = await ordersMockService.GetOrderAsync(1);

        Assert.IsNotNull(order);
    }

    [TestMethod]
    public async Task GetFakeOrdersTest()
    {
        var ordersMockService = new OrderMockService();
        var result = await ordersMockService.GetOrdersAsync();

        Assert.AreNotEqual(result.Count(), 0);
    }
}
