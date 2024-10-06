namespace Ordering.UnitTests.Domain;
public class PaymentMethodUnitTests
{
    public class Ctor
    {
        [Theory, AutoNSubstituteData]
        public void WhenValid_CreatePaymentMethod(
            CardType cardType,
            string alias,
            string cardNumber,
            string securityNumber,
            string cardHolderName)
        {
            // Arrange

            //Act

            PaymentMethod _ = new(cardType, alias, cardNumber, securityNumber, cardHolderName, DateTime.Now.AddYears(1));

            //Assert

        }

        [Theory, AutoNSubstituteData]
        public void WhenCardNumberEmpty_ThrowDomainException(
            CardType cardType,
            string alias,
            string securityNumber,
            string cardHolderName)
        {
            // Arrange

            //Act

            PaymentMethod func() => new(cardType, alias, null, securityNumber, cardHolderName, DateTime.Now.AddYears(1));

            //Assert

            Assert.Throws<OrderingDomainException>(func);
        }

        [Theory, AutoNSubstituteData]
        public void WhenCardHasExpired_ThrowDomainException(
            CardType cardType,
            string alias,
            string cardNumber,
            string securityNumber,
            string cardHolderName)
        {
            // Arrange

            //Act

            PaymentMethod func() => new(cardType, alias, cardNumber, securityNumber, cardHolderName, DateTime.Now.AddYears(-1));

            //Assert

            Assert.Throws<OrderingDomainException>(func);
        }
    }
}
