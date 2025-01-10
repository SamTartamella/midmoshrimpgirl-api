using Microsoft.AspNetCore.Mvc;
using midmoshrimpgirl_api.Models.Responses;

namespace midmoshrimpgirl_api.Controllers
{
    public interface IProductApi
    {
        Task<IActionResult> GetProductByUrlString(string productSearchString);

        IActionResult GetProductByName(string productName);
    }
}
