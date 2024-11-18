using Aspire.Hosting.Dapr;
using eShop.AppHost;
using Microsoft.Extensions.Configuration;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);
builder.AddForwardedHeaders();

IResourceBuilder<RedisResource> redis = builder.AddRedis("redis");

//IResourceBuilder<ParameterResource> rabbitMqPassword = builder.AddParameter("rabbitMqDefaultPassword");
IResourceBuilder<RabbitMQServerResource> rabbitMq = builder.AddRabbitMQ("eventbus")
    .WithManagementPlugin()
    .WithEndpoint("tcp", e => e.Port = 5672)
    .WithEndpoint("management", e => e.Port = 15672);

IResourceBuilder<PostgresServerResource> pg = builder.AddPostgres("postgres")
    .WithImage("ankane/pgvector")
    .WithImageTag("latest");

IResourceBuilder<PostgresDatabaseResource> catalogDb = pg.AddDatabase("catalogdb");
IResourceBuilder<PostgresDatabaseResource> customerDb = pg.AddDatabase("customerdb");
IResourceBuilder<PostgresDatabaseResource> identityDb = pg.AddDatabase("identitydb");
IResourceBuilder<PostgresDatabaseResource> orderDb = pg.AddDatabase("orderingdb");
IResourceBuilder<PostgresDatabaseResource> webhooksDb = pg.AddDatabase("webhooksdb");

string launchProfileName = ShouldUseHttpForEndpoints() ? "http" : "https";

// Services
IResourceBuilder<ProjectResource> identityApi = builder.AddProject<Projects.eShop_Identity_API>("identity-api", launchProfileName)
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppProtocol = launchProfileName
    })
    .WithExternalHttpEndpoints()
    .WithReference(identityDb)
    .WaitFor(identityDb);

EndpointReference identityEndpoint = identityApi.GetEndpoint(launchProfileName);

IResourceBuilder<ProjectResource> basketApi = builder.AddProject<Projects.eShop_Basket_API>("basket-api")
    .WithDaprSidecar()
    .WithReference(redis)
    .WithReference(rabbitMq)
    .WithEnvironment("Identity__Url", identityEndpoint)
    .WaitFor(redis)
    .WaitFor(rabbitMq)
    .WaitFor(identityApi);

IResourceBuilder<ProjectResource> catalogApi = builder.AddProject<Projects.eShop_Catalog_API>("catalog-api")
    .WithDaprSidecar()
    .WithReference(rabbitMq)
    .WithReference(catalogDb)
    .WaitFor(rabbitMq)
    .WaitFor(catalogDb);

IResourceBuilder<ProjectResource> customerApi = builder.AddProject<Projects.eShop_Customer_API>("customer-api")
    .WithDaprSidecar()
    .WithReference(customerDb)
    .WithEnvironment("Identity__Url", identityEndpoint)
    .WaitFor(customerDb);

//IResourceBuilder<IDaprComponentResource> pubSub = builder.AddDaprPubSub("pubSub")
    //.WaitFor(rabbitMq);

IResourceBuilder<IDaprComponentResource> pubSub = builder.AddDaprPubSub("pubsub",
    new DaprComponentOptions
    {
        LocalPath = "./components/pubsub.yaml"
    })
    .WaitFor(rabbitMq);

IResourceBuilder<ProjectResource> orderingApi = builder.AddProject<Projects.eShop_Ordering_API>("ordering-api")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        LogLevel = "debug"
    })
    .WithReference(rabbitMq)
    .WithReference(orderDb)
    .WithReference(pubSub)
    .WithEnvironment("Identity__Url", identityEndpoint);

IResourceBuilder<ProjectResource> masterDataApi = builder.AddProject<Projects.eShop_MasterData_API>("masterdata-api")
    .WithDaprSidecar()
    .WithEnvironment("Identity__Url", identityEndpoint);

builder.AddProject<Projects.eShop_OrderProcessor>("order-processor")
    .WithReference(rabbitMq)
    .WithReference(orderDb)
    .WaitFor(rabbitMq);

builder.AddProject<Projects.eShop_PaymentProcessor>("payment-processor")
    .WithReference(rabbitMq);    

IResourceBuilder<ProjectResource> webHooksApi = builder.AddProject<Projects.eShop_Webhooks_API>("webhooks-api")
    .WithReference(rabbitMq)
    .WithReference(webhooksDb)
    .WithEnvironment("Identity__Url", identityEndpoint)
    .WaitFor(webhooksDb)
    .WaitFor(rabbitMq);

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
    .WithDaprSidecar()
    .WithExternalHttpEndpoints()
    .WithReference(basketApi)
    .WithReference(catalogApi)
    .WithReference(customerApi)
    .WithReference(orderingApi)
    .WithReference(rabbitMq)
    .WithEnvironment("IdentityUrl", identityEndpoint)
    .WaitFor(rabbitMq)
    .WaitFor(catalogApi);

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

IResourceBuilder<ProjectResource> adminApp = builder.AddProject<Projects.eShop_AdminApp>("admin-app")
    .WithDaprSidecar()
    .WithExternalHttpEndpoints()
    .WithReference(catalogApi)
    .WithReference(customerApi)
    .WithReference(orderingApi)
    .WithReference(masterDataApi)
    .WithEnvironment("IdentityUrl", identityEndpoint)
    .WaitFor(catalogApi)
    .WaitFor(customerApi)
    .WaitFor(orderingApi)
    .WaitFor(masterDataApi);

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
    string? envValue = Environment.GetEnvironmentVariable(EnvVarName);

    // Attempt to parse the environment variable value; return true if it's exactly "1".
    return int.TryParse(envValue, out int result) && result == 1;
}
