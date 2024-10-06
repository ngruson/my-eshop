namespace eShop.Ordering.UnitTests.Domain;

public class BuyerAggregateTest
{
    public BuyerAggregateTest()
    { }

    [Fact]
    public void Create_buyer_item_success()
    {
        //Arrange    
        var identity = new Guid().ToString();
        var name = "fakeUser";

        //Act 
        var fakeBuyerItem = new Buyer(identity, name);

        //Assert
        Assert.NotNull(fakeBuyerItem);
    }

    [Fact]
    public void Create_buyer_item_fail()
    {
        //Arrange    
        var identity = string.Empty;
        var name = "fakeUser";

        //Act - Assert
        Assert.Throws<ArgumentNullException>(() => new Buyer(identity, name));
    }

    [Theory, AutoNSubstituteData]
    public void add_payment_success(
        CardType cardType)
    {
        //Arrange    
        string alias = "fakeAlias";
        string cardNumber = "124";
        string securityNumber = "1234";
        string cardHolderName = "FakeHolderNAme";
        DateTime expiration = DateTime.UtcNow.AddYears(1);
        int orderId = 1;
        string name = "fakeUser";
        string identity = new Guid().ToString();
        Buyer fakeBuyerItem = new(identity, name);

        //Act
        PaymentMethod result = fakeBuyerItem.VerifyOrAddPaymentMethod(cardType, alias, cardNumber, securityNumber, cardHolderName, expiration, orderId);

        //Assert
        Assert.NotNull(result);
    }

    [Theory, AutoNSubstituteData]
    public void create_payment_method_success(
        CardType cardType)
    {
        //Arrange    
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.UtcNow.AddYears(1);

        //Act
        var result = new PaymentMethod(cardType, alias, cardNumber, securityNumber, cardHolderName, expiration);

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
        CardType cardType)
    {
        //Arrange    
        string alias = "fakeAlias";
        int orderId = 1;
        string cardNumber = "12";
        string cardSecurityNumber = "123";
        string cardHolderName = "FakeName";
        DateTime cardExpiration = DateTime.UtcNow.AddYears(1);
        int expectedResult = 1;
        string name = "fakeUser";

        //Act 
        Buyer fakeBuyer = new(Guid.NewGuid().ToString(), name);
        fakeBuyer.VerifyOrAddPaymentMethod(cardType, alias, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration, orderId);

        //Assert
        Assert.Equal(fakeBuyer.DomainEvents.Count, expectedResult);
    }
}
