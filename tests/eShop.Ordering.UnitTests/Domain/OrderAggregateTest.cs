namespace eShop.Ordering.UnitTests.Domain;

using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

public class OrderAggregateTest
{
    public OrderAggregateTest()
    { }

    [Fact]
    public void Create_order_item_success()
    {
        //Arrange    
        var productId = 1;
        var productName = "FakeProductName";
        var unitPrice = 12;
        var discount = 15;
        var pictureUrl = "FakeUrl";
        var units = 5;

        //Act 
        var fakeOrderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        //Assert
        Assert.NotNull(fakeOrderItem);
    }

    [Fact]
    public void Invalid_number_of_units()
    {
        //Arrange    
        var productId = 1;
        var productName = "FakeProductName";
        var unitPrice = 12;
        var discount = 15;
        var pictureUrl = "FakeUrl";
        var units = -1;

        //Act - Assert
        Assert.Throws<OrderingDomainException>(() => new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units));
    }

    [Fact]
    public void Invalid_total_of_order_item_lower_than_discount_applied()
    {
        //Arrange    
        var productId = 1;
        var productName = "FakeProductName";
        var unitPrice = 12;
        var discount = 15;
        var pictureUrl = "FakeUrl";
        var units = 1;
        
        //Act - Assert
        Assert.Throws<OrderingDomainException>(() => new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units));       
    }

    [Fact]
    public void Invalid_discount_setting()
    {
        //Arrange    
        var productId = 1;
        var productName = "FakeProductName";
        var unitPrice = 12;
        var discount = 15;
        var pictureUrl = "FakeUrl";
        var units = 5;

        //Act 
        var fakeOrderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        //Assert
        Assert.Throws<OrderingDomainException>(() => fakeOrderItem.SetNewDiscount(-1));
    }

    [Fact]
    public void Invalid_units_setting()
    {
        //Arrange    
        var productId = 1;
        var productName = "FakeProductName";
        var unitPrice = 12;
        var discount = 15;
        var pictureUrl = "FakeUrl";
        var units = 5;

        //Act 
        var fakeOrderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        //Assert
        Assert.Throws<OrderingDomainException>(() => fakeOrderItem.AddUnits(-1));
    }

    [Theory, AutoNSubstituteData]
    public void when_add_two_times_on_the_same_item_then_the_total_of_order_should_be_the_sum_of_the_two_items(
        Order order,
        int productId,
        string productName,
        decimal unitPrice)
    {
        // Arrange

        // Act

        order.AddOrderItem(productId, productName, unitPrice, 0, null, 1);
        order.AddOrderItem(productId, productName, unitPrice, 0, null, 1);

        // Assert

        Assert.Equal(unitPrice * 2, order.GetTotal());
    }

    [Theory, AutoNSubstituteData]
    public void Add_new_Order_raises_new_event(
        CardType cardType)
    {
        //Arrange
        string street = "fakeStreet";
        string city = "FakeCity";
        string state = "fakeState";
        string country = "fakeCountry";
        string zipCode = "FakeZipCode";
        string cardNumber = "12";
        string cardSecurityNumber = "123";
        string cardHolderName = "FakeName";
        DateTime cardExpiration = DateTime.UtcNow.AddYears(1);
        int expectedResult = 1;

        //Act 
        Order fakeOrder = new("1", "fakeName", new Address(street, city, state, country, zipCode), cardType, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);

        //Assert
        Assert.Equal(fakeOrder.DomainEvents.Count, expectedResult);
    }

    [Theory, AutoNSubstituteData]
    public void Add_event_Order_explicitly_raises_new_event(
        CardType cardType)
    {
        //Arrange   
        string street = "fakeStreet";
        string city = "FakeCity";
        string state = "fakeState";
        string country = "fakeCountry";
        string zipCode = "FakeZipCode";
        string cardNumber = "12";
        string cardSecurityNumber = "123";
        string cardHolderName = "FakeName";
        DateTime cardExpiration = DateTime.UtcNow.AddYears(1);
        int expectedResult = 2;

        //Act 
        Order fakeOrder = new("1", "fakeName", new Address(street, city, state, country, zipCode), cardType, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);
        fakeOrder.AddDomainEvent(new OrderStartedDomainEvent(fakeOrder, "fakeName", "1", cardType, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration));
        //Assert
        Assert.Equal(fakeOrder.DomainEvents.Count, expectedResult);
    }

    [Theory, AutoNSubstituteData]
    public void Remove_event_Order_explicitly(
        CardType cardType)
    {
        //Arrange    
        string street = "fakeStreet";
        string city = "FakeCity";
        string state = "fakeState";
        string country = "fakeCountry";
        string zipCode = "FakeZipCode";
        string cardNumber = "12";
        string cardSecurityNumber = "123";
        string cardHolderName = "FakeName";
        DateTime cardExpiration = DateTime.UtcNow.AddYears(1);
        Order fakeOrder = new("1", "fakeName", new Address(street, city, state, country, zipCode), cardType, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);
        OrderStartedDomainEvent fakeEvent = new(fakeOrder, "1", "fakeName", cardType, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);
        int expectedResult = 1;

        //Act         
        fakeOrder.AddDomainEvent(fakeEvent);
        fakeOrder.RemoveDomainEvent(fakeEvent);
        //Assert
        Assert.Equal(fakeOrder.DomainEvents.Count, expectedResult);
    }
}
