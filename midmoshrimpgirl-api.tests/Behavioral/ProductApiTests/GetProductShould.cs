using Dapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using midmoshrimpgirl_api.Controllers;
using midmoshrimpgirl_api.dataAccess.Models;
using midmoshrimpgirl_api.dataAccess.Repositories;
using midmoshrimpgirl_api.dataAccess.Wrappers.Dapper;
using midmoshrimpgirl_api.Models.Responses;
using midmoshrimpgirl_domain.DataAccess;
using midmoshrimpgirl_domain.Queries;
using NSubstitute;
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

        [TestInitialize]
        public void Init()
        {
            _dapperWrapperMock = Substitute.For<IDapperWrapper>();
            _getProductRepository = new GetProductRepository(_dapperWrapperMock);
            _getProduct = new GetProduct(_getProductRepository);
            _sut = new ProductApi(_getProduct);

            _productId = RandomValue.Int();
        }


        [TestMethod]
        public async Task ReturnProductResponse_WhenCalledGivenProductId()
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
                Arg.Is<DynamicParameters>((p) => p.Get<int>("@ProductId") == _productId)).Returns([expectedResult]);

            // Act 
            var result = await _sut.GetProduct(_productId) as ObjectResult;


            // Assert 
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(expectedResult);
            await _dapperWrapperMock.Received(1).ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProduct"),
                Arg.Is<DynamicParameters>((p) => p.Get<int>("@ProductId") == _productId));

        }

        [TestMethod]
        public async Task Return404_WhenProductNotFound()
        {
            // Arrange          
            _dapperWrapperMock.ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProduct"),
                Arg.Is<DynamicParameters>((p) => p.Get<int>("ProductId") == _productId)).Returns([]);

            // Act 
            var result = await _sut.GetProduct(_productId) as ObjectResult;

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}