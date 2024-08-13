namespace Ordering.UnitTests.Domain;
public class PaymentMethodUnitTests
{
    public class Ctor
    {
        [Theory, AutoNSubstituteData]
        public void WhenValid_CreatePaymentMethod(
            int cardTypeId,
            string alias,
            string cardNumber,
            string securityNumber,
            string cardHolderName)
        {
            // Arrange

            //Act

            PaymentMethod _ = new(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, DateTime.Now.AddYears(1));

            //Assert

        }

        [Theory, AutoNSubstituteData]
        public void WhenCardNumberEmpty_ThrowDomainException(
            int cardTypeId,
            string alias,
            string securityNumber,
            string cardHolderName)
        {
            // Arrange

            //Act

            PaymentMethod func() => new(cardTypeId, alias, null, securityNumber, cardHolderName, DateTime.Now.AddYears(1));

            //Assert

            Assert.Throws<OrderingDomainException>(func);
        }

        [Theory, AutoNSubstituteData]
        public void WhenCardHasExpired_ThrowDomainException(
            int cardTypeId,
            string alias,
            string cardNumber,
            string securityNumber,
            string cardHolderName)
        {
            // Arrange

            //Act

            PaymentMethod func() => new(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, DateTime.Now.AddYears(-1));

            //Assert

            Assert.Throws<OrderingDomainException>(func);
        }
    }
}
