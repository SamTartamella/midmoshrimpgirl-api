using Microsoft.AspNetCore.Mvc;
using midmoshrimpgirl_api.Extensions;
using midmoshrimpgirl_domain.Models.Exceptions;
using midmoshrimpgirl_domain.Queries;
using System.ComponentModel.DataAnnotations;
using NullReferenceException = midmoshrimpgirl_domain.Models.Exceptions.NullReferenceException;

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
        [Route("products/{productUrlSearchString}")]
        public async Task<IActionResult> GetProductByUrlString([Required][FromRoute] string productUrlSearchString)
        {
            try
            {
                if (string.IsNullOrEmpty(productUrlSearchString))
                {
                    return StatusCode(400, "Product URL Search String may not be null or empty.");
                }

                var product = await _GetProduct.WithSearchString(productUrlSearchString);
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

        [HttpGet]
        [Route("{productName}")]
        public async Task<IActionResult> GetProductByName([Required][FromRoute] string productName)
        {
            try
            {
                if (string.IsNullOrEmpty(productName))
                {
                    return StatusCode(400, "Product Name may not be null or empty.");
                }

                var product = await _GetProduct.WithName(productName);
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
        }
    }
}
