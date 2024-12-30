using Microsoft.AspNetCore.Mvc;
using midmoshrimpgirl_api.Extensions;
using midmoshrimpgirl_api.Models.Exceptions;
using midmoshrimpgirl_api.Models.Responses;
using midmoshrimpgirl_domain.Models;
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
        [Route("products/{productSearchString}")]
        public async Task<IActionResult> GetProduct([Required][FromRoute] string productSearchString)
        {
            try
            {
                var product = await _GetProduct.WithSearchString(productSearchString);
                return Ok(product.ToApiResponse());
            }
            catch (SQLException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (NotFoundException ex)
            {
                return StatusCode(404, ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
