using Microsoft.AspNetCore.Mvc;
using midmoshrimpgirl_api.Models.Responses;
using System.ComponentModel.DataAnnotations;

namespace midmoshrimpgirl_api.Controllers
{
    [ApiController]
    public class ProductApi : ControllerBase, IProductApi
    {
        public ProductApi() { }

        [HttpGet]
        [Route("products/{productId}")]
        public async Task<ProductResponse> GetProduct([Required][FromRoute] int productId)
        {
            return await Task.FromResult(new ProductResponse()
            {
                Name = "Shrimp",
                Price = 1.11m,
                ImageLink = "https://localhost/images/123"
            });
        }
    }
}
