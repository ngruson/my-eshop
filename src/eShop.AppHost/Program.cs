using eShop.AppHost;
using Microsoft.Extensions.Configuration;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);
builder.AddForwardedHeaders();

IResourceBuilder<RedisResource> redis = builder.AddRedis("redis");
IResourceBuilder<RabbitMQServerResource> rabbitMq = builder.AddRabbitMQ("eventbus");
IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
    .WithImage("ankane/pgvector")
    .WithImageTag("latest");

IResourceBuilder<PostgresDatabaseResource> catalogDb = postgres.AddDatabase("catalogdb");
IResourceBuilder<PostgresDatabaseResource> customerDb = postgres.AddDatabase("customerdb");
IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identitydb");
IResourceBuilder<PostgresDatabaseResource> orderDb = postgres.AddDatabase("orderingdb");
IResourceBuilder<PostgresDatabaseResource> webhooksDb = postgres.AddDatabase("webhooksdb");

string launchProfileName = ShouldUseHttpForEndpoints() ? "http" : "https";

// Services
IResourceBuilder<ProjectResource> identityApi = builder.AddProject<Projects.eShop_Identity_API>("identity-api", launchProfileName)
    .WithExternalHttpEndpoints()
    .WithReference(identityDb);

EndpointReference identityEndpoint = identityApi.GetEndpoint(launchProfileName);

IResourceBuilder<ProjectResource> basketApi = builder.AddProject<Projects.eShop_Basket_API>("basket-api")
    .WithReference(redis)
    .WithReference(rabbitMq)
    .WithEnvironment("Identity__Url", identityEndpoint);

IResourceBuilder<ProjectResource> catalogApi = builder.AddProject<Projects.eShop_Catalog_API>("catalog-api")
    .WithReference(rabbitMq)
    .WithReference(catalogDb);

IResourceBuilder<ProjectResource> customerApi = builder.AddProject<Projects.eShop_Customer_API>("customer-api")
    .WithReference(customerDb)
    .WithEnvironment("Identity__Url", identityEndpoint);

IResourceBuilder<ProjectResource> orderingApi = builder.AddProject<Projects.eShop_Ordering_API>("ordering-api")
    .WithReference(rabbitMq)
    .WithReference(orderDb)
    .WithEnvironment("Identity__Url", identityEndpoint);

IResourceBuilder<ProjectResource> masterDataApi = builder.AddProject<Projects.eShop_MasterData_API>("masterData-api")
    .WithEnvironment("Identity__Url", identityEndpoint);

builder.AddProject<Projects.eShop_OrderProcessor>("order-processor")
    .WithReference(rabbitMq)
    .WithReference(orderDb);

builder.AddProject<Projects.eShop_PaymentProcessor>("payment-processor")
    .WithReference(rabbitMq);

IResourceBuilder<ProjectResource> webHooksApi = builder.AddProject<Projects.eShop_Webhooks_API>("webhooks-api")
    .WithReference(rabbitMq)
    .WithReference(webhooksDb)
    .WithEnvironment("Identity__Url", identityEndpoint);

// Reverse proxies
builder.AddProject<Projects.eShop_Mobile_Bff_Shopping>("mobile-bff")
    .WithReference(catalogApi)
    .WithReference(orderingApi)
    .WithReference(basketApi)
    .WithReference(identityApi);

// Apps
IResourceBuilder<ProjectResource> webhooksClient = builder.AddProject<Projects.eShop_WebhookClient>("webhooksclient", launchProfileName)
    .WithReference(webHooksApi)
    .WithEnvironment("IdentityUrl", identityEndpoint);

IResourceBuilder<ProjectResource> webApp = builder.AddProject<Projects.eShop_WebApp>("webapp", launchProfileName)
    .WithExternalHttpEndpoints()
    .WithReference(basketApi)
    .WithReference(catalogApi)
    .WithReference(customerApi)
    .WithReference(orderingApi)
    .WithReference(rabbitMq)
    .WithEnvironment("IdentityUrl", identityEndpoint);

// set to true if you want to use OpenAI
bool useOpenAI = false;
if (useOpenAI)
{
    const string openAIName = "openai";
    const string textEmbeddingName = "text-embedding-3-small";
    const string chatModelName = "gpt-35-turbo-16k";

    // to use an existing OpenAI resource, add the following to the AppHost user secrets:
    // "ConnectionStrings": {
    //   "openai": "Key=<API Key>" (to use https://api.openai.com/)
    //     -or-
    //   "openai": "Endpoint=https://<name>.openai.azure.com/" (to use Azure OpenAI)
    // }
    IResourceBuilder<IResourceWithConnectionString> openAI;
    if (builder.Configuration.GetConnectionString(openAIName) is not null)
    {
        openAI = builder.AddConnectionString(openAIName);
    }
    else
    {
        // to use Azure provisioning, add the following to the AppHost user secrets:
        // "Azure": {
        //   "SubscriptionId": "<your subscription ID>"
        //   "Location": "<location>"
        // }
        openAI = builder.AddAzureOpenAI(openAIName)
            .AddDeployment(new AzureOpenAIDeployment(chatModelName, "gpt-35-turbo", "0613"))
            .AddDeployment(new AzureOpenAIDeployment(textEmbeddingName, "text-embedding-3-small", "1"));
    }

    catalogApi
        .WithReference(openAI)
        .WithEnvironment("AI__OPENAI__EMBEDDINGNAME", textEmbeddingName);

    webApp
        .WithReference(openAI)
        .WithEnvironment("AI__OPENAI__CHATMODEL", chatModelName);
}

var adminApp = builder.AddProject<Projects.eShop_AdminApp>("admin-app")
    .WithExternalHttpEndpoints()
    .WithReference(catalogApi)
    .WithReference(customerApi)
    .WithReference(orderingApi)
    .WithReference(masterDataApi)
    .WithEnvironment("IdentityUrl", identityEndpoint);

// Wire up the callback urls (self referencing)
webApp.WithEnvironment("CallBackUrl", webApp.GetEndpoint(launchProfileName));
webhooksClient.WithEnvironment("CallBackUrl", webhooksClient.GetEndpoint(launchProfileName));
adminApp.WithEnvironment("CallBackUrl", adminApp.GetEndpoint(launchProfileName));

// Identity has a reference to all of the apps for callback URLs, this is a cyclic reference
identityApi.WithEnvironment("AdminAppClient", adminApp.GetEndpoint(launchProfileName))
    .WithEnvironment("BasketApiClient", basketApi.GetEndpoint("http"))
    .WithEnvironment("CustomerApiClient", customerApi.GetEndpoint("http"))
    .WithEnvironment("MasterDataApiClient", masterDataApi.GetEndpoint("http"))
    .WithEnvironment("OrderingApiClient", orderingApi.GetEndpoint("http"))
    .WithEnvironment("WebAppClient", webApp.GetEndpoint(launchProfileName))
    .WithEnvironment("WebhooksApiClient", webHooksApi.GetEndpoint("http"))
    .WithEnvironment("WebhooksWebClient", webhooksClient.GetEndpoint(launchProfileName));

builder.Build().Run();

// For test use only.
// Looks for an environment variable that forces the use of HTTP for all the endpoints. We
// are doing this for ease of running the Playwright tests in CI.
static bool ShouldUseHttpForEndpoints()
{
    const string EnvVarName = "ESHOP_USE_HTTP_ENDPOINTS";
    var envValue = Environment.GetEnvironmentVariable(EnvVarName);

    // Attempt to parse the environment variable value; return true if it's exactly "1".
    return int.TryParse(envValue, out int result) && result == 1;
}
