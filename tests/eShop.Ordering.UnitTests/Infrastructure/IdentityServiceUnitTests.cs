using System.Security.Claims;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;

namespace eShop.Ordering.UnitTests.Infrastructure;

public class IdentityServiceUnitTests
{
    public class GetUserIdentity
    {
        [Theory, AutoNSubstituteData]
        public void return_id_when_sub_claim_exists(
            [Substitute, Frozen] IHttpContextAccessor httpContextAccessor,
            IdentityService sut,
            ClaimsPrincipal user,
            Guid sub
        )
        {
            // Arrange

            ClaimsIdentity claimsIdentity = new();
            claimsIdentity.AddClaim(new Claim("sub", sub.ToString()));
            user.AddIdentity(claimsIdentity);
            httpContextAccessor.HttpContext.User.Returns(user);

            // Act

            Guid? result = sut.GetUserIdentity();

            // Assert

            Assert.Equal(sub, result);
        }

        [Theory, AutoNSubstituteData]
        public void return_null_when_sub_claim_does_not_exist(
            [Substitute, Frozen] IHttpContextAccessor httpContextAccessor,
            IdentityService sut,
            ClaimsPrincipal user            
        )
        {
            // Arrange

            //ClaimsIdentity claimsIdentity = new();            
            //user.AddIdentity(claimsIdentity);
            httpContextAccessor.HttpContext.User.Returns(user);

            // Act

            Guid? result = sut.GetUserIdentity();

            // Assert

            Assert.Null(result);
        }
    }
}
