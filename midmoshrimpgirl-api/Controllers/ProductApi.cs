using Microsoft.AspNetCore.Mvc;
using midmoshrimpgirl_api.Models.Responses;
using midmoshrimpgirl_domain.Queries;
using System.ComponentModel.DataAnnotations;

namespace midmoshrimpgirl_api.Controllers
{
    [ApiController]
    public class ProductApi : ControllerBase, IProductApi
    {
        private readonly IGetProduct _GetProduct;

        public ProductApi(IGetProduct getProduct)
        {
            _GetProduct = getProduct;
        }

        [HttpGet]
        [Route("products/{productId}")]
        public async Task<ProductResponse> GetProduct([Required][FromRoute] int productId)
        {
            var product = await _GetProduct.WithId(productId);

            return await Task.FromResult(new ProductResponse()
            {
                Name = "Shrimp",
                Price = 1.11m,
                ImageLink = "https://localhost/images/123"
            });
        }
    }
}
