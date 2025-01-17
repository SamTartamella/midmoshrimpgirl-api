using Dapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using midmoshrimpgirl_api.dataAccess.Models;
using midmoshrimpgirl_api.Models.Responses;
using NSubstitute;
using RandomTestValues;

namespace midmoshrimpgirl_api.tests.Behavioral.ProductApiTests;

[TestClass]
[TestCategory("Unit")]
public class GetProductByNameShould : ProductApiTestBase
{
    protected string _productNameInput;

    [TestInitialize]
    public override void Init()
    {
        base.Init();


    }

    [TestMethod]
    public async Task ReturnProductResponse_WhenCalledGivenProduceName()
    {
        // Arrange
        var expectedResult = new DatabaseProduct()
        {
            Name = RandomValue.String(),
            Price = RandomValue.Decimal(),
            ImageLink = RandomValue.String()
        };

        var expectedResponse = new ProductResponse()
        {
            Name = expectedResult.Name,
            Price = expectedResult.Price,
            ImageLink = expectedResult.ImageLink
        };

        _dapperWrapperMock.ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProductByName"),
            Arg.Is<DynamicParameters>((p) => p.Get<string>("@ProductName") == _productNameInput))
            .Returns([expectedResult]);

        // Act 
        var result = await _sut.GetProductByName(_productNameInput) as ObjectResult;
        var resultData = (ProductResponse)result.Value;

        // Assert 
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        resultData.Should().BeEquivalentTo(expectedResponse);
        await _dapperWrapperMock.Received(1).ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProductByName"),
            Arg.Is<DynamicParameters>((p) => p.Get<string>(@"ProductName") == _productNameInput));

    }
}
