using System.Net;
using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using NSubstitute;
using Swashbuckle.AspNetCore.SwaggerGen;
using static eShop.ServiceDefaults.ConfigureSwaggerOptions;

namespace eShop.ServiceDefaults.UnitTests;
public class ConfigureSwaggerOptionsUnitTests
{
    private static readonly string[] scopes = ["basket"];

    [Theory, AutoNSubstituteData]
    internal void Configure_WithValidConfig_OAuthConfigured(
        [Substitute, Frozen] IApiVersionDescriptionProvider mockProvider)
    {
        // Arrange

        mockProvider.ApiVersionDescriptions
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
            mockProvider,
            configuration);

        // Act

        SwaggerGenOptions options = new();

        sut.Configure(options);

        // Assert

        Assert.Single(options.SwaggerGeneratorOptions.SecuritySchemes);
        Assert.True(options.SwaggerGeneratorOptions.SecuritySchemes.ContainsKey("oauth2"));

        Assert.Equivalent(scopes, options.OperationFilterDescriptors[0].Arguments[0]);
    }

    [Theory, AutoNSubstituteData]
    internal void Configure_WithoutIdentityConfig_OAuthNotConfigured(
        [Substitute, Frozen] IApiVersionDescriptionProvider mockProvider)
    {
        // Arrange

        mockProvider.ApiVersionDescriptions
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
            mockProvider,
            configuration);

        // Act

        SwaggerGenOptions options = new();

        sut.Configure(options);

        // Assert

        Assert.Empty(options.SwaggerGeneratorOptions.SecuritySchemes);
        Assert.Empty(options.OperationFilterDescriptors);
    }

    [Theory, AutoNSubstituteData]
    internal void Configure_WithValidConfig_DocumentIsSetCorrectly(
        [Substitute, Frozen] IApiVersionDescriptionProvider mockProvider)
    {
        // Arrange

        mockProvider.ApiVersionDescriptions
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
            mockProvider,
            configuration);

        // Act

        SwaggerGenOptions options = new();

        sut.Configure(options);

        // Assert

        Assert.Equal("1.0", options.SwaggerGeneratorOptions.SwaggerDocs["v1"].Version);
        Assert.Equal("documentDescription. This API version has been deprecated.", options.SwaggerGeneratorOptions.SwaggerDocs["v1"].Description);
        Assert.Equal("documentTitle", options.SwaggerGeneratorOptions.SwaggerDocs["v1"].Title);
    }

    [Theory, AutoNSubstituteData]
    internal void Configure_ApiIsDeprecated_DocumentIsSetCorrectly(
        [Substitute, Frozen] IApiVersionDescriptionProvider mockProvider)
    {
        // Arrange

        mockProvider.ApiVersionDescriptions
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
            mockProvider,
            configuration);

        // Act

        SwaggerGenOptions options = new();

        sut.Configure(options);

        // Assert

        Assert.Equal("documentDescription. This API version has been deprecated.", options.SwaggerGeneratorOptions.SwaggerDocs["v1"].Description);
    }

    [Theory, AutoNSubstituteData]
    internal void Configure_ApiIsSunset_DocumentIsSetCorrectly(
        [Substitute, Frozen] IApiVersionDescriptionProvider mockProvider,
        Uri uri)
    {
        // Arrange

        SunsetPolicy sunsetPolicy = new(DateTime.Now.AddDays(30));

        sunsetPolicy.Links.Add(
            new(uri, "sunset")
            {
                Type = "text/html"
            });

        sunsetPolicy.Links.Add(
            new(uri, "sunset")
            {
                Type = "text/html",
                Title = "title"
            });

        mockProvider.ApiVersionDescriptions
            .Returns(
            [
                new(apiVersion: new(1, 0), groupName: "v1", deprecated: true, 
                    sunsetPolicy
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
            mockProvider,
            configuration);

        // Act

        SwaggerGenOptions options = new();

        sut.Configure(options);

        // Assert

        Assert.NotEmpty(options.SwaggerGeneratorOptions.SwaggerDocs["v1"].Description);
    }

    public class AuthorizeCheckOperationFilterUnitTests
    {
        [Theory, AutoNSubstituteData]
        internal void WithEndpointMetadata_AddOperationsAndSecurity(
        List<IAuthorizeData> authorizeData,
        ISchemaGenerator schemaGenerator,
        SchemaRepository schemaRepository,
        MethodInfo methodInfo,
        string[] scopes
        )
        {
            // Arrange

            ApiDescription apiDescription = new()
            {
                ActionDescriptor = new()
                {
                    EndpointMetadata = authorizeData.Cast<object>().ToList()
                }

            };

            OpenApiOperation operation = new();

            OperationFilterContext context = new(apiDescription, schemaGenerator, schemaRepository, methodInfo);

            AuthorizeCheckOperationFilter sut = new(scopes);

            // Act

            sut.Apply(operation, context);

            // Assert

            Assert.Contains(operation.Responses, x => x.Key == ((int)HttpStatusCode.Unauthorized).ToString());
            Assert.Contains(operation.Responses, x => x.Key == ((int)HttpStatusCode.Forbidden).ToString());

            Assert.Single(operation.Security);
            Assert.Equal(operation.Security[0].First().Value, scopes);
        }

        [Theory, AutoNSubstituteData]
        internal void WithoutEndpointMetadata_AddNoOperationsAndSecurity(
        ISchemaGenerator schemaGenerator,
        SchemaRepository schemaRepository,
        MethodInfo methodInfo,
        string[] scopes
        )
        {
            // Arrange

            ApiDescription apiDescription = new()
            {
                ActionDescriptor = new()
            };

            OpenApiOperation operation = new();

            OperationFilterContext context = new(apiDescription, schemaGenerator, schemaRepository, methodInfo);

            AuthorizeCheckOperationFilter sut = new(scopes);

            // Act

            sut.Apply(operation, context);

            // Assert

            Assert.Empty(operation.Responses);
            Assert.Empty(operation.Security);
        }
    }
}
