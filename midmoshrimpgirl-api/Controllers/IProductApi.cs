using Microsoft.AspNetCore.Mvc;

namespace midmoshrimpgirl_api.Controllers
{
    public interface IProductApi
    {
        Task<IActionResult> GetProductByUrlString(string productSearchString);

        Task <IActionResult> GetProductByName(string productName);
    }
}
