using MediatR;

namespace eShop.Shared.UnitTests.Behaviors;

public class TestRequest : IRequest<TestResponse>
{
    public string? Name { get; set; }
}
