using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace eShop.Ordering.UnitTests.Domain;

public class BuyerAggregateTest
{
    public BuyerAggregateTest()
    { }

    [Theory, AutoNSubstituteData]
    public void create_buyer_item_success(Guid identity, string userName, string buyerName)
    {
        //Arrange

        //Act

        Buyer fakeBuyerItem = new(identity, userName, buyerName);

        //Assert
        Assert.NotNull(fakeBuyerItem);
    }

    [Theory, AutoNSubstituteData]
    public void Create_buyer_item_fail(string userName, string buyerName)
    {
        //Arrang        

        //Act - Assert

        Assert.Throws<ArgumentNullException>(() => new Buyer(Guid.Empty, userName, buyerName));
    }

    [Theory, AutoNSubstituteData]
    public void add_payment_success(
        Order order,
        Buyer buyer,
        CardType cardType)
    {
        // Arrange

        string alias = "fakeAlias";
        string cardNumber = "124";
        string securityNumber = "1234";
        string cardHolderName = "FakeHolderNAme";
        DateTime expiration = DateTime.UtcNow.AddYears(1);

        // Act

        PaymentMethod result = buyer.VerifyOrAddPaymentMethod(cardType, alias, cardNumber, securityNumber, cardHolderName, expiration, order);

        // Assert

        Assert.NotNull(result);
    }

    [Theory, AutoNSubstituteData]
    public void create_payment_method_success(
        CardType cardType)
    {
        //Arrange

        //Act

        PaymentMethod result = new(cardType, "fakeAlias", "124", "1234", "FakeHolderName", DateTime.UtcNow.AddYears(1));

        //Assert
        Assert.NotNull(result);
    }

    [Theory, AutoNSubstituteData]
    public void create_payment_method_expiration_fail(
        CardType cardType)
    {
        //Arrange    
        string alias = "fakeAlias";
        string cardNumber = "124";
        string securityNumber = "1234";
        string cardHolderName = "FakeHolderNAme";
        DateTime expiration = DateTime.UtcNow.AddYears(-1);

        //Act - Assert
        Assert.Throws<OrderingDomainException>(() => new PaymentMethod(cardType, alias, cardNumber, securityNumber, cardHolderName, expiration));
    }

    [Theory, AutoNSubstituteData]
    public void payment_method_isEqualTo(
        CardType cardType)
    {
        //Arrange    
        string alias = "fakeAlias";
        string cardNumber = "124";
        string securityNumber = "1234";
        string cardHolderName = "FakeHolderNAme";
        DateTime expiration = DateTime.UtcNow.AddYears(1);

        //Act
        PaymentMethod fakePaymentMethod = new(cardType, alias, cardNumber, securityNumber, cardHolderName, expiration);
        bool result = fakePaymentMethod.IsEqualTo(cardType, cardNumber, expiration);

        //Assert
        Assert.True(result);
    }

    [Theory, AutoNSubstituteData]
    public void Add_new_paymentMethod_raises_new_event(
        Order order,
        Buyer buyer,
        CardType cardType)
    {
        // Arrange

        string alias = "fakeAlias";
        string cardNumber = "12";
        string cardSecurityNumber = "123";
        string cardHolderName = "FakeName";
        DateTime cardExpiration = DateTime.UtcNow.AddYears(1);
        int expectedResult = 1;

        //Act 
        
        buyer.VerifyOrAddPaymentMethod(cardType, alias, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration, order);

        //Assert
        Assert.Equal(buyer.DomainEvents.Count, expectedResult);
    }
}
