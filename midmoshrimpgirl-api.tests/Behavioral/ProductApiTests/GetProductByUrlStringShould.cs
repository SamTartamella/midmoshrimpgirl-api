using Dapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using midmoshrimpgirl_api.dataAccess.Models;
using midmoshrimpgirl_api.Models.Responses;
using midmoshrimpgirl_domain.Models;
using midmoshrimpgirl_domain.Models.Exceptions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RandomTestValues;

namespace midmoshrimpgirl_api.tests.Behavioral.ProductApiTests
{
    [TestClass]
    [TestCategory("Unit")]
    public class GetProductByUrlStringShould : ProductApiTestBase
    {
        protected int _productId;
        protected string _productSearchString;

        [TestInitialize]
        public override void Init()
        {
            base.Init();

            _productId = RandomValue.Int();
            _productSearchString = RandomValue.String();
        }

        [TestMethod]
        public async Task ReturnProductResponse_WhenCalledGivenProductSearchString()
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

            _dapperWrapperMock.ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProduct"),
                Arg.Is<DynamicParameters>((p) => p.Get<string>("@ProductSearchString") == _productSearchString))
                .Returns([expectedResult]);

            // Act 
            var result = await _sut.GetProductByUrlString(_productSearchString) as ObjectResult;
            var resultData = (ProductResponse)result.Value;


            // Assert 
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            resultData.Should().BeEquivalentTo(expectedResponse);
            await _dapperWrapperMock.Received(1).ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProduct"),
                Arg.Is<DynamicParameters>((p) => p.Get<string>("@ProductSearchString") == _productSearchString));

        }

        [TestMethod]
        public async Task Return404_WhenProductNotFound()
        {
            // Arrange
            var expectedException = new NotFoundException("Product not found.");
            _dapperWrapperMock.ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProduct"),
                Arg.Is<DynamicParameters>((p) => p.Get<string>("@ProductSearchString") == _productSearchString)).Returns([]);

            // Act 
            var result = await _sut.GetProductByUrlString(_productSearchString) as ObjectResult;
            var resultMessage = result.Value;

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            resultMessage.Should().Be(expectedException.Message);
        }

        [TestMethod]
        public async Task Return500_OnSQLError()
        {
            // Arrange 
            var expectedException = new SQLException("Error encountered in SQL.");
            _dapperWrapperMock.ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProduct"),
                Arg.Is<DynamicParameters>((p) => p.Get<string>("@ProductSearchString") == _productSearchString)).Throws(expectedException);

            // Act 
            var result = await _sut.GetProductByUrlString(_productSearchString) as ObjectResult;
            var resultMessage = result.Value;

            // Assert 
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            resultMessage.Should().Be("Error encountered in SQL.");
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        public async Task Return500_WhenDomainProductNameIsNullOrEmpty(string name)
        {
            // Arrange 
            var expectedResult = new DatabaseProduct()
            {
                Name = name,
                Price = RandomValue.Decimal(),
                ImageLink = RandomValue.String()
            };

            _dapperWrapperMock.ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProduct"),
                Arg.Is<DynamicParameters>((p) => p.Get<string>("@ProductSearchString") == _productSearchString)).Returns([expectedResult]);

            // Act 
            var result = await _sut.GetProductByUrlString(_productSearchString) as ObjectResult;
            var resultMessage = result.Value;

            // Assert 
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            resultMessage.Should().Be("Product name is empty.");

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

            _dapperWrapperMock.ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProduct"),
                Arg.Is<DynamicParameters>((p) => p.Get<string>("@ProductSearchString") == _productSearchString))
                .Returns([expectedDatabaseResult]);

            // Act 
            var result = await _sut.GetProductByUrlString(_productSearchString) as ObjectResult;
            var resultMessage = (ProductResponse)result.Value;

            // Assert 
            resultMessage.ImageLink.Should().Be(expectedApiResponse.ImageLink);

            await _dapperWrapperMock.Received(1).ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProduct"),
                Arg.Is<DynamicParameters>((p) => p.Get<string>("@ProductSearchString") == _productSearchString));

        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        public async Task Return400_GivenNulOrEmptyString(string productSearchString)
        {
            // Arrange 
            _productSearchString = "";

            // Act 
            var result = await _sut.GetProductByUrlString(_productSearchString) as ObjectResult;

            // Assert 
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        }
    }
}