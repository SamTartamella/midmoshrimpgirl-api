using midmoshrimpgirl_domain.Models;

namespace midmoshrimpgirl_domain.DataAccess
{
    public interface IGetProductRepository
    {
        public Task<DomainProductResponse> GetBySearchString(string productSearchString);
    }
}
