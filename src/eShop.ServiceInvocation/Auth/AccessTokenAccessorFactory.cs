using Duende.AccessTokenManagement;
using eShop.ServiceInvocation.Options;
using eShop.Shared.DI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using static Duende.IdentityModel.OidcConstants;

namespace eShop.ServiceInvocation.Auth;

public class AccessTokenAccessorFactory(ServiceProviderWrapper serviceProvider, IOptions<ServiceInvocationOptions> options)
{
    public virtual IAccessTokenAccessor Create()
    {
        switch (options.Value.GrantType)
        {
            case GrantTypes.ClientCredentials:
                IClientCredentialsTokenManagementService tokenService =
                    serviceProvider.GetRequiredService<IClientCredentialsTokenManagementService>();

                return new ClientCredentialsAccessTokenAccessor(tokenService, options);

            default:
                IHttpContextAccessor httpContextAccessor =
                    serviceProvider.GetRequiredService<IHttpContextAccessor>();

                return new HttpContextAccessTokenAccessor(httpContextAccessor);
        }
    }
}
