using eShop.Catalog.API.Infrastructure.Exceptions;
using eShop.Catalog.API.Model;

namespace eShop.Catalog.UnitTests.Model;

public class CatalogItemUnitTests
{
    public class RemoveStock
    {
        [Theory, AutoNSubstituteData]
        internal void when_valid_input_decrease_available_stock(
        CatalogItem sut,
        int quantity)
        {
            // Arrange

            int original = sut.AvailableStock;
            int removed = Math.Min(quantity, sut.AvailableStock);

            // Act

            int result = sut.RemoveStock(quantity);

            // Assert

            Assert.Equal(removed, result);
            Assert.Equal(original - removed, sut.AvailableStock);
        }

        [Theory, AutoNSubstituteData]
        internal void when_no_stock_throw_exception(
            CatalogItem sut,
            int quantity)
        {
            // Arrange

            sut.AvailableStock = 0;

            // Act

            void act() => sut.RemoveStock(quantity);

            // Assert

            Assert.Throws<CatalogDomainException>(act);
        }

        [Theory, AutoNSubstituteData]
        internal void when_quantity_invalid_throw_exception(
            CatalogItem sut,
            int quantity)
        {
            // Arrange

            quantity = 0;

            // Act

            void act() => sut.RemoveStock(quantity);

            // Assert

            Assert.Throws<CatalogDomainException>(act);
        }
    }

    public class AddStock
    {
        [Theory, AutoNSubstituteData]
        internal void when_max_stock_threshold_not_exceeded_add_stock_by_requested_quantity(
        CatalogItem sut,
        int quantity)
        {
            // Arrange

            sut.MaxStockThreshold = Math.Max(quantity, sut.AvailableStock) * 2;

            // Act

            sut.AddStock(quantity);

            // Assert

            Assert.True(sut.AvailableStock > 0);
            Assert.False(sut.OnReorder);
        }

        [Theory, AutoNSubstituteData]
        internal void when_max_stock_threshold_exceeded_add_stock_to_max(
        CatalogItem sut,
        int quantity)
        {
            // Arrange

            sut.MaxStockThreshold = Math.Max(quantity, sut.AvailableStock);

            // Act

            sut.AddStock(quantity);

            // Assert

            Assert.True(sut.AvailableStock > 0);
            Assert.False(sut.OnReorder);
        }
    }
}
