using FluentValidation;

namespace eShop.Shared.UnitTests.Behaviors;

internal class TestRequestValidator : AbstractValidator<TestRequest>
{
    public TestRequestValidator()
    {
        this.RuleFor(order => order.Name).NotEmpty();
    }
}
