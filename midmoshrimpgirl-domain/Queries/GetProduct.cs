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

        public async Task<DomainProductResponse> WithId(int productId)
        {
            var domainProduct = await _getProductRepository.GetById(productId);
            return domainProduct;
        }
    }
}
