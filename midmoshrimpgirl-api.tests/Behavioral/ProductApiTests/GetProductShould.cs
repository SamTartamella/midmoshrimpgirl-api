using Dapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using midmoshrimpgirl_api.Controllers;
using midmoshrimpgirl_api.dataAccess.Models;
using midmoshrimpgirl_api.dataAccess.Repositories;
using midmoshrimpgirl_api.dataAccess.Wrappers.Dapper;
using midmoshrimpgirl_api.Models.Exceptions;
using midmoshrimpgirl_api.Models.Responses;
using midmoshrimpgirl_domain.DataAccess;
using midmoshrimpgirl_domain.Models;
using midmoshrimpgirl_domain.Queries;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RandomTestValues;
using RandomTestValues.Formats;

namespace midmoshrimpgirl_api.tests.Behavioral.ProductApiTests
{
    [TestClass]
    [TestCategory("Unit")]
    public class GetProductShould
    {
        protected IDapperWrapper _dapperWrapperMock;
        protected IGetProduct _getProduct;
        protected IGetProductRepository _getProductRepository;

        protected int _productId;
        protected ProductApi _sut;
        protected string _productSearchString;

        [TestInitialize]
        public void Init()
        {
            _dapperWrapperMock = Substitute.For<IDapperWrapper>();
            _getProductRepository = new GetProductRepository(_dapperWrapperMock);
            _getProduct = new GetProduct(_getProductRepository);
            _sut = new ProductApi(_getProduct);

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
                Arg.Is<DynamicParameters>((p) => p.Get<string>("@ProductSearchString") == _productSearchString)).Returns([expectedResult]);

            // Act 
            var result = await _sut.GetProduct(_productSearchString) as ObjectResult;


            // Assert 
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(expectedResult);
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
            var result = await _sut.GetProduct(_productSearchString) as ObjectResult;
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
            var result = await _sut.GetProduct(_productSearchString) as ObjectResult;
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
            var result = await _sut.GetProduct(_productSearchString) as ObjectResult;
            var resultMessage = result.Value;

            // Assert 
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            resultMessage.Should().Be("No product attributes may be empty.");

        }

        //[TestMethod]
        //[DataRow("")]
        //[DataRow(null)]
        //public async Task ReturnDefaultImage_WhenDomainProductImageLinkIsNullOrEmpty(string link)
        //{
        //    // Arrange 
        //    var expectedDefaultImage = RandomValue.String(); //this needs to be in config

        //    var expectedDatabaseResult = new DatabaseProduct()
        //    {
        //        Name = RandomValue.String(),
        //        Price = RandomValue.Decimal(),
        //        ImageLink = link
        //    };

        //    var expectedApiResponse = new DomainProductResponse()
        //    {
        //        Name = expectedDatabaseResult.Name,
        //        Price = expectedDatabaseResult.Price,
        //        ImageLink = 
        //    }

        //    _dapperWrapperMock.ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProduct"),
        //        Arg.Is<DynamicParameters>((p) => p.Get<string>("@ProductSearchString") == _productSearchString)).Returns([expectedDatabaseResult]);

        //    // Act 
        //    var result = await _sut.GetProduct(_productSearchString) as ObjectResult;
        //    var resultMessage = result.Value;

        //    // Assert 
        //    result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        //    //resultMessage.Should().Be("No product attributes may be empty.");
        //    //Assert that we are getting expected ApiResponse that has default return image

        //}

    }
}