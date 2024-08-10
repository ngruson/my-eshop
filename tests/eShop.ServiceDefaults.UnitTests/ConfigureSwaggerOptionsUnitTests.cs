using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Configuration;
using Moq;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace eShop.ServiceDefaults.UnitTests;
public class ConfigureSwaggerOptionsUnitTests
{
    private static readonly string[] scopes = ["basket"];

    [Theory]
    [AutoMoqData]
    internal void Configure_WithValidConfig_OAuthConfigured(
        [Frozen] Mock<IApiVersionDescriptionProvider> mockProvider)
    {
        // Arrange

        mockProvider.Setup(x => x.ApiVersionDescriptions)
            .Returns(
            [
                new(apiVersion: new(1, 0), groupName: "v1", deprecated: false)
            ]);

        Dictionary<string, string> inMemorySettings = new()
        {
            { "Identity:Url", "http://identity" },
            { "Identity:Scopes:basket", "Basket API" },
            { "OpenApi:Document:Title", "documentTitle" },
            { "OpenApi:Document:Description", "documentDescription" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        ConfigureSwaggerOptions sut = new(
            mockProvider.Object,
            configuration);

        // Act

        SwaggerGenOptions options = new();

        sut.Configure(options);

        // Assert

        Assert.Single(options.SwaggerGeneratorOptions.SecuritySchemes);
        Assert.True(options.SwaggerGeneratorOptions.SecuritySchemes.ContainsKey("oauth2"));

        Assert.Equivalent(scopes, options.OperationFilterDescriptors[0].Arguments[0]);
    }

    [Theory]
    [AutoMoqData]
    internal void Configure_WithoutIdentityConfig_OAuthNotConfigured(
        [Frozen] Mock<IApiVersionDescriptionProvider> mockProvider)
    {
        // Arrange

        mockProvider.Setup(x => x.ApiVersionDescriptions)
            .Returns(
            [
                new(apiVersion: new(1, 0), groupName: "v1", deprecated: false)
            ]);

        Dictionary<string, string> inMemorySettings = new()
        {
            { "OpenApi:Document:Title", "documentTitle" },
            { "OpenApi:Document:Description", "documentDescription" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        ConfigureSwaggerOptions sut = new(
            mockProvider.Object,
            configuration);

        // Act

        SwaggerGenOptions options = new();

        sut.Configure(options);

        // Assert

        Assert.Empty(options.SwaggerGeneratorOptions.SecuritySchemes);
        Assert.Empty(options.OperationFilterDescriptors);
    }

    [Theory]
    [AutoMoqData]
    internal void Configure_WithValidConfig_DocumentIsSetCorrectly(
        [Frozen] Mock<IApiVersionDescriptionProvider> mockProvider)
    {
        // Arrange

        mockProvider.Setup(x => x.ApiVersionDescriptions)
            .Returns(
            [
                new(apiVersion: new(1, 0), groupName: "v1", deprecated: true)
            ]);

        Dictionary<string, string> inMemorySettings = new()
        {
            { "OpenApi:Document:Title", "documentTitle" },
            { "OpenApi:Document:Description", "documentDescription" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        ConfigureSwaggerOptions sut = new(
            mockProvider.Object,
            configuration);

        // Act

        SwaggerGenOptions options = new();

        sut.Configure(options);

        // Assert

        Assert.Equal("1.0", options.SwaggerGeneratorOptions.SwaggerDocs["v1"].Version);
        Assert.Equal("documentDescription. This API version has been deprecated.", options.SwaggerGeneratorOptions.SwaggerDocs["v1"].Description);
        Assert.Equal("documentTitle", options.SwaggerGeneratorOptions.SwaggerDocs["v1"].Title);
    }

    [Theory]
    [AutoMoqData]
    internal void Configure_ApiIsDeprecated_DocumentIsSetCorrectly(
        [Frozen] Mock<IApiVersionDescriptionProvider> mockProvider)
    {
        // Arrange

        mockProvider.Setup(x => x.ApiVersionDescriptions)
            .Returns(
            [
                new(apiVersion: new(1, 0), groupName: "v1", deprecated: true)
            ]);

        Dictionary<string, string> inMemorySettings = new()
        {
            { "OpenApi:Document:Title", "documentTitle" },
            { "OpenApi:Document:Description", "documentDescription" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        ConfigureSwaggerOptions sut = new(
            mockProvider.Object,
            configuration);

        // Act

        SwaggerGenOptions options = new();

        sut.Configure(options);

        // Assert

        Assert.Equal("documentDescription. This API version has been deprecated.", options.SwaggerGeneratorOptions.SwaggerDocs["v1"].Description);
    }

    [Theory]
    [AutoMoqData]
    internal void Configure_ApiIsSunset_DocumentIsSetCorrectly(
        [Frozen] Mock<IApiVersionDescriptionProvider> mockProvider,
        DateTimeOffset sunsetDate)
    {
        // Arrange

        mockProvider.Setup(x => x.ApiVersionDescriptions)
            .Returns(
            [
                new(apiVersion: new(1, 0), groupName: "v1", deprecated: true, 
                    new SunsetPolicy(sunsetDate)
                )
            ]);

        Dictionary<string, string> inMemorySettings = new()
        {
            { "OpenApi:Document:Title", "documentTitle" },
            { "OpenApi:Document:Description", "documentDescription" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        ConfigureSwaggerOptions sut = new(
            mockProvider.Object,
            configuration);

        // Act

        SwaggerGenOptions options = new();

        sut.Configure(options);

        // Assert

        Assert.Equal($"documentDescription. This API version has been deprecated. The API will be sunset on {sunsetDate.Date.ToShortDateString()}.",
            options.SwaggerGeneratorOptions.SwaggerDocs["v1"].Description);
    }
}
