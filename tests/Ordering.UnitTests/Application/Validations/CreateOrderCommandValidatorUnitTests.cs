using eShop.Ordering.API.Application.Models;
using eShop.Ordering.API.Application.Validations;
using FluentValidation.TestHelper;

namespace Ordering.UnitTests.Application.Validations;
public class CreateOrderCommandValidatorUnitTests
{
    [Theory, AutoNSubstituteData]
    internal void valid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.True(result.IsValid);
    }

    [Theory, AutoNSubstituteData]
    internal void city_empty_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            city: null,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.City), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void street_empty_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            command.City,
            street: null,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.Street), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void state_empty_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            state: null,
            command.Country,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.State), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void country_empty_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            country: null,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.Country), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void zipCode_empty_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            zipcode: null,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.ZipCode), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void cardNumber_empty_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber: null,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.CardNumber), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void cardNumber_too_short_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string shorter than 12 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 11)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber: null,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.CardNumber), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void cardNumber_too_long_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string longer than 20 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 21)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber: null,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.CardNumber), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void cardHolderName_empty_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber,
            cardHolderName: null,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.CardHolderName), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void expiration_date_empty_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.MinValue,
            cardSecurityNumber,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.CardExpiration), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void expiration_date_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(-1),
            cardSecurityNumber,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.CardExpiration), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void card_security_number_empty_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber: null,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.CardSecurityNumber), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void card_security_number_too_short_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        // Random string shorter than 3 characters
        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 2)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.CardSecurityNumber), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void card_security_number_too_long_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        // Random string longer than 3 characters
        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.CardSecurityNumber), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void card_type_id_empty_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        List<BasketItem> basketItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            basketItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            0
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.CardTypeId), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void order_items_empty_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            [],
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardTypeId
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.OrderItems), result.Errors.Select(_ => _.PropertyName));
    }
}
