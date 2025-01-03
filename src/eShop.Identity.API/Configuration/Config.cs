namespace eShop.Identity.API.Configuration;

internal class Config
{
    // ApiResources define the apis in your system
    public static IEnumerable<ApiResource> GetApis()
    {
        return
        [
            new ApiResource("customers", "Customer Service"),
            new ApiResource("basket", "Basket Service"),
            new ApiResource("masterData", "Master Data Service"),
            new ApiResource("orders", "Orders Service"),
            new ApiResource("webapp", "Web Application"),
            new ApiResource("webhooks", "Webhooks registration Service"),
            new ApiResource("workflows", "Workflow Service"),
        ];
    }

    // ApiScope is used to protect the API 
    //The effect is the same as that of API resources in IdentityServer 3.x
    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return
        [
            new ApiScope("customers", "Customer Service"),
            new ApiScope("basket", "Basket Service"),
            new ApiScope("masterData", "Master Data Service"),
            new ApiScope("order-status", "Order Status"),
            new ApiScope("orders", "Orders Service"),
            new ApiScope("webhooks", "Webhooks registration Service"),
            new ApiScope("workflows", "Workflow Service"),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        ];
    }

    // Identity resources are data like user ID, name, or email address of a user
    // see: http://docs.identityserver.io/en/release/configuration/resources.html
    public static IEnumerable<IdentityResource> GetResources()
    {
        return
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        ];
    }

    // client want to access resources (aka scopes)
    public static IEnumerable<Client> GetClients(IConfiguration configuration)
    {
        return
        [
            new Client
            {
                ClientId = "maui",
                ClientName = "eShop MAUI OpenId Client",
                AllowedGrantTypes = GrantTypes.Code,                    
                //Used to retrieve the access token on the back channel.
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                RedirectUris = { configuration["MauiCallback"]! },
                RequireConsent = false,
                RequirePkce = true,
                PostLogoutRedirectUris = { $"{configuration["MauiCallback"]}/Account/Redirecting" },
                //AllowedCorsOrigins = { "http://eshopxamarin" },
                AllowedScopes =
                [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "orders",
                    "basket",
                    "mobileshoppingagg",
                    "webhooks"                    
                ],
                //Allow requesting refresh tokens for long lived API access
                AllowOfflineAccess = true,
                AllowAccessTokensViaBrowser = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AccessTokenLifetime = 60*60*2, // 2 hours
                IdentityTokenLifetime= 60*60*2 // 2 hours
            },
            new Client
            {
                ClientId = "webapp",
                ClientName = "WebApp Client",
                ClientSecrets =
                [
                    new Secret("secret".Sha256())
                ],
                ClientUri = $"{configuration["WebAppClient"]}",                             // public uri of the client
                AllowedGrantTypes = GrantTypes.Code,
                AllowAccessTokensViaBrowser = false,
                RequireConsent = false,
                AllowOfflineAccess = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                RequirePkce = false,
                RedirectUris =
                [
                    $"{configuration["WebAppClient"]}/signin-oidc"
                ],
                PostLogoutRedirectUris =
                [
                    $"{configuration["WebAppClient"]}/signout-callback-oidc"
                ],
                AllowedScopes =
                [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "orders",
                    "basket",
                    "webshoppingagg",
                    "webhooks",
                    "workflows"
                ],
                AccessTokenLifetime = 60*60*2, // 2 hours
                IdentityTokenLifetime= 60*60*2 // 2 hours
            },
            new Client
            {
                ClientId = "webhooksclient",
                ClientName = "Webhooks Client",
                ClientSecrets =
                [
                    new Secret("secret".Sha256())
                ],
                ClientUri = $"{configuration["WebhooksWebClient"]}",                             // public uri of the client
                AllowedGrantTypes = GrantTypes.Code,
                AllowAccessTokensViaBrowser = false,
                RequireConsent = false,
                AllowOfflineAccess = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                RedirectUris =
                [
                    $"{configuration["WebhooksWebClient"]}/signin-oidc"
                ],
                PostLogoutRedirectUris =
                [
                    $"{configuration["WebhooksWebClient"]}/signout-callback-oidc"
                ],
                AllowedScopes =
                [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "webhooks"
                ],
                AccessTokenLifetime = 60*60*2, // 2 hours
                IdentityTokenLifetime= 60*60*2 // 2 hours
            },
            new Client
            {
                ClientId = "basketswaggerui",
                ClientName = "Basket Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = { $"{configuration["BasketApiClient"]}/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"{configuration["BasketApiClient"]}/swagger/" },

                AllowedScopes =
                {
                    "basket"
                }
            },
            new Client
            {
                ClientId = "customerswaggerui",
                ClientName = "Customer Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = { $"{configuration["CustomerApiClient"]}/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"{configuration["CustomerApiClient"]}/swagger/" },
                AllowedScopes =
                {
                    "customers"
                }
            },
            new Client
            {
                ClientId = "masterDataSwaggerUI",
                ClientName = "Master Data Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = { $"{configuration["MasterDataApiClient"]}/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"{configuration["MasterDataApiClient"]}/swagger/" },

                AllowedScopes =
                {
                    "masterData"
                }
            },
            new Client
            {
                ClientId = "orderingswaggerui",
                ClientName = "Ordering Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = { $"{configuration["OrderingApiClient"]}/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"{configuration["OrderingApiClient"]}/swagger/" },

                AllowedScopes =
                {
                    "orders"
                }
            },
            new Client
            {
                ClientId = "webhooksswaggerui",
                ClientName = "WebHooks Service Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = { $"{configuration["WebhooksApiClient"]}/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"{configuration["WebhooksApiClient"]}/swagger/" },

                AllowedScopes =
                {
                    "webhooks"
                }
            },
            new Client
            {
                ClientId = "adminApp",
                ClientName = "Admin Client",
                ClientSecrets =
                [
                    new Secret("secret".Sha256())
                ],
                ClientUri = $"{configuration["AdminAppClient"]}",
                AllowedGrantTypes = GrantTypes.Code,
                AllowAccessTokensViaBrowser = false,
                RequireConsent = false,
                AllowOfflineAccess = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                RequirePkce = false,
                RedirectUris =
                [
                    $"{configuration["AdminAppClient"]}/signin-oidc"
                ],
                PostLogoutRedirectUris =
                [
                    $"{configuration["AdminAppClient"]}/signout-callback-oidc"
                ],
                AllowedScopes =
                [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.LocalApi.ScopeName,
                    "orders",
                    "basket"
                ],
                AccessTokenLifetime = 60*60*2, // 2 hours
                IdentityTokenLifetime= 60*60*2 // 2 hours
            },
            new Client
            {
                ClientId = "invoicing",
                ClientName = "Invoicing Client",
                ClientSecrets =
                [
                    new Secret("secret".Sha256())
                ],
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = ["orders"]
            },
            new Client
            {
                ClientId = "order-processor",
                ClientName = "Order Processor Client",
                ClientSecrets =
                [
                    new Secret("secret".Sha256())
                ],
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = ["workflows"]
            },
            new Client
            {
                ClientId = "payment-processor",
                ClientName = "Payment Processor Client",
                ClientSecrets =
                [
                    new Secret("secret".Sha256())
                ],
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = ["workflows"]
            },
            new Client
            {
                ClientId = "workflow",
                ClientName = "Workflow Client",
                ClientSecrets =
                [
                    new Secret("secret".Sha256())
                ],
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = ["orders"]
            },
            new Client
            {
                ClientId = "workflowswaggerui",
                ClientName = "Workflow Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = { $"{configuration["WorkflowApiClient"]}/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"{configuration["WorkflowApiClient"]}/swagger/" },
                AllowedScopes =
                {
                    "workflows"
                }
            },
        ];
    }
}
