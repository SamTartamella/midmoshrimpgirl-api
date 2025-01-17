using midmoshrimpgirl_domain.DataAccess;
using midmoshrimpgirl_domain.Models;

namespace midmoshrimpgirl_domain.Queries
{
    public class GetProduct : IGetProduct
    {
        protected IGetProductRepository _getProductRepository;

        public GetProduct(IGetProductRepository getProductRepository)
        {
            _getProductRepository = getProductRepository;
        }

        public async Task<DomainProductResponse> WithSearchString(string productSearchString)
        {
            var domainProduct = await _getProductRepository.GetBySearchString(productSearchString);
            return domainProduct;
        }

        public async Task<DomainProductResponse> WithName(string productName)
        {
            var domainProduct = await _getProductRepository.GetByName(productName);
            return domainProduct;
        }
    }
}
