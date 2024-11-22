using Dapper;
using FluentAssertions;
using midmoshrimpgirl_api.Controllers;
using midmoshrimpgirl_api.dataAccess.Models;
using midmoshrimpgirl_api.dataAccess.Repositories;
using midmoshrimpgirl_api.dataAccess.Wrappers.Dapper;
using midmoshrimpgirl_domain.DataAccess;
using midmoshrimpgirl_domain.Queries;
using NSubstitute;
using RandomTestValues;

namespace midmoshrimpgirl_api.tests.Behavioral.ProductApiTests
{
    [TestClass]
    [TestCategory("Unit")]
    public class GetProductShould
    {
        protected IDapperWrapper _dapperWrapperMock;
        protected IGetProduct _getProduct;
        protected IGetProductRepository _getProductRepository;

        [TestMethod]
        public async Task ReturnProductResponse_WhenCalledGivenProductId()
        {
            // Arrange
            var productId = RandomValue.Int();
            var expectedResult = new DatabaseProduct()
            {
                Name = "Shrimp",
                Price = 1.11m,
                ImageLink = "https://localhost/images/123"
            };

            _dapperWrapperMock = Substitute.For<IDapperWrapper>();
            _getProductRepository = new GetProductRepository(_dapperWrapperMock);
            _getProduct = new GetProduct(_getProductRepository);
            var sut = new ProductApi(_getProduct);

            _dapperWrapperMock.ExecuteStoredProcedure<DatabaseProduct>(Arg.Is<string>("GetProduct"),
                Arg.Is<DynamicParameters>((p) => p.Get<int>("@ProductId") == productId)).Returns([expectedResult]);

            // Act 
            var result = await sut.GetProduct(productId);

            // Assert 
            result.Should().BeEquivalentTo(expectedResult);
            await _dapperWrapperMock.Received(1).ExecuteStoredProcedure<object>(Arg.Is<string>("GetProduct"),
                Arg.Is<DynamicParameters>((p) => p.Get<int>("@MessageId") == productId));

        }
    }
}