using FluentAssertions;
using midmoshrimpgirl_api.Controllers;
using midmoshrimpgirl_api.Models.Responses;
using RandomTestValues;

namespace midmoshrimpgirl_api.tests.Behavioral.ProductApiTests
{
    [TestClass]
    [TestCategory("Unit")]
    public class GetProductShould
    {
        [TestMethod]
        public async Task ReturnProductResponse_WhenCalled()
        {
            // Arrange
            int productId = RandomValue.Int();
            ProductApi sut = new ProductApi();
            ProductResponse expectedResult = new ProductResponse()
            {
                Name = "Shrimp",
                Price = 1.11m,
                ImageLink = "https://localhost/images/123"
            };

            // Act 
            Models.Responses.ProductResponse result = await sut.GetProduct(productId);

            // Assert 
            _ = result.Should().BeEquivalentTo(expectedResult);

        }
    }
}