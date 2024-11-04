using eShop.Ordering.API.Application.Commands.CreateOrder;
using eShop.Ordering.API.Application.Validations;
using eShop.Ordering.Contracts.CreateOrder;
using FluentValidation.TestHelper;

namespace eShop.Ordering.UnitTests.Application.Validations;
public class CreateOrderCommandValidatorUnitTests
{
    [Theory, AutoNSubstituteData]
    internal void valid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
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
            command.CardType
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
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
            command.UserId,
            command.UserName,
            City: null,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardType
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
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
            command.UserId,
            command.UserName,
            command.City,
            Street: null,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardType
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
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            State: null,
            command.Country,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardType
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
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            Country: null,
            command.ZipCode,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardType
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
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            ZipCode: null,
            cardNumber,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardType
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
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            CardNumber: null,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardType
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
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string shorter than 12 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 11)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            CardNumber: null,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardType
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
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string longer than 20 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 21)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            CardNumber: null,
            command.CardHolderName,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardType
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
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
            command.UserId,
            command.UserName,
            command.City,
            command.Street,
            command.State,
            command.Country,
            command.ZipCode,
            cardNumber,
            CardHolderName: null,
            DateTime.Now.AddYears(1),
            cardSecurityNumber,
            command.CardType
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
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
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
            command.CardType
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
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
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
            command.CardType
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
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
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
            CardSecurityNumber: null,
            command.CardType
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
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        // Random string shorter than 3 characters
        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 2)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
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
            command.CardType
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
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        // Random string longer than 3 characters
        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
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
            command.CardType
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.CardSecurityNumber), result.Errors.Select(_ => _.PropertyName));
    }

    [Theory, AutoNSubstituteData]
    internal void card_type_empty_invalid(
        CreateOrderCommandValidator sut,
        CreateOrderCommand command,
        OrderItemDto[] orderItems
    )
    {
        // Arrange

        // Create AutoFixture customization later

        // Random string between 12 and 19 characters
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        CreateOrderCommand copy = new(
            orderItems,
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
            Guid.Empty
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.CardType), result.Errors.Select(_ => _.PropertyName));
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
        string cardNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());

        string cardSecurityNumber = new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 3)
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
            command.CardType
        );

        // Act

        TestValidationResult<CreateOrderCommand> result = sut.TestValidate(copy);

        //Assert

        Assert.False(result.IsValid);
        Assert.Contains(nameof(command.OrderItems), result.Errors.Select(_ => _.PropertyName));
    }
}
