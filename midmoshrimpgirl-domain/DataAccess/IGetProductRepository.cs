using midmoshrimpgirl_domain.Models;

namespace midmoshrimpgirl_domain.DataAccess
{
    public interface IGetProductRepository
    {
        public Task<DomainProductResponse> GetById(int productId);
    }
}
