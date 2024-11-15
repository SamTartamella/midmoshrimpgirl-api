using midmoshrimpgirl_api.Models.Responses;

namespace midmoshrimpgirl_api.Controllers
{
    public interface IProductApi
    {
        Task<ProductResponse> GetProduct(int productId);
    }
}
