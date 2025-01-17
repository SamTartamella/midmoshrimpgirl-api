using Dapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using midmoshrimpgirl_api.dataAccess.Models;
using midmoshrimpgirl_api.Models.Responses;
using midmoshrimpgirl_domain.Models;
using midmoshrimpgirl_domain.Models.Exceptions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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

        _productNameInput = RandomValue.String();
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

    [TestMethod]
    public async Task Return404_WhenProductNotFound()
    {
        // Arrange 
        var expectedException = new NotFoundException("Product not found.");
        _dapperWrapperMock.ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProductByName"),
            Arg.Is<DynamicParameters>((p) => p.Get<string>("@ProductName") == _productNameInput))
            .Returns([]);

        // Act 
        var result = await _sut.GetProductByName(_productNameInput) as ObjectResult;
        var resultData = result.Value;

        // Assert 
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        resultData.Should().Be(expectedException.Message);
    }

    [TestMethod]
    public async Task Return500_OnSQLError()
    {
        // Arrange 
        var expectedException = new SQLException("Error encountered in SQL.");
        _dapperWrapperMock.ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProductByName"),
            Arg.Is<DynamicParameters>((p) => p.Get<string>("@ProductName") == _productNameInput)).Throws(expectedException);

        // Act 
        var result = await _sut.GetProductByName(_productNameInput) as ObjectResult;
        var resultData = result.Value;

        // Assert 
        result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        resultData.Should().Be("Error encountered in SQL.");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public async Task Return400_GivenNulOrEmptyString(string productNameInput)
    {
        // Arrange 
        _productNameInput = "";

        // Act 
        var result = await _sut.GetProductByName(_productNameInput) as ObjectResult;

        // Assert 
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public async Task ReturnDefaultImage_WhenDomainProductImageLinkIsNullOrEmpty(string link)
    {
        var expectedDatabaseResult = new DatabaseProduct()
        {
            Name = RandomValue.String(),
            Price = RandomValue.Decimal(),
            ImageLink = link
        };

        var expectedApiResponse = new DomainProductResponse()
        {
            Name = expectedDatabaseResult.Name,
            Price = expectedDatabaseResult.Price,
            ImageLink = _mockDefaultImageLink
        };

        _dapperWrapperMock.ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProductByName"),
            Arg.Is<DynamicParameters>((p) => p.Get<string>("@ProductName") == _productNameInput))
            .Returns([expectedDatabaseResult]);

        // Act 
        var result = await _sut.GetProductByName(_productNameInput) as ObjectResult;
        var resultData = (ProductResponse)result.Value;

        // Assert 
        resultData.ImageLink.Should().Be(expectedApiResponse.ImageLink);
        await _dapperWrapperMock.Received(1).ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProductByName"),
            Arg.Is<DynamicParameters>((p) => p.Get<string>(@"ProductName") == _productNameInput));
    }
}