namespace eShop.Workflow.UnitTests;

public class AssessStockItemsActivityUnitTests
{
    // WorkflowActivityContext is not mockable yet, coming soon
    // https://github.com/dapr/dotnet-sdk/pull/1358

    //[Theory, AutoNSubstituteData]
    //internal async Task return_success_when_activity_succeeds(
    //    [Substitute, Frozen] WorkflowActivityContext workflowContext,
    //    [Substitute, Frozen] ICatalogApiClient catalogApiClient,
    //    AssessStockItemsForOrderRequestDto assessStockItemsForOrderRequestDto,
    //    AssessStockItemsForOrderResponseDto assessStockItemsForOrderResponseDto,
    //    AssessStockItemsActivityInput assessStockItemsActivityInput,
    //    AssessStockItemsActivity sut)
    //{
    //    // Arrange        

    //    catalogApiClient.AssessStockItemsForOrder(assessStockItemsForOrderRequestDto)
    //        .Returns(assessStockItemsForOrderResponseDto);

    //    // Act

    //    Result<AssessStockItemsForOrderResponseDto> result = await sut.RunAsync(workflowContext, assessStockItemsActivityInput);

    //    // Assert

    //    Assert.True(result.IsSuccess);
    //}
}
