using Dapper;
using midmoshrimpgirl_api.dataAccess.Models;
using midmoshrimpgirl_api.dataAccess.Wrappers.Dapper;
using midmoshrimpgirl_api.Models.Exceptions;
using midmoshrimpgirl_domain.DataAccess;
using midmoshrimpgirl_domain.Models;

namespace midmoshrimpgirl_api.dataAccess.Repositories
{
    public class GetProductRepository : IGetProductRepository
    {
        private readonly IDapperWrapper _dapperWrapper;
        private readonly string _defaultImageLink;
        private readonly string _nonConfigDefaultImageLink; 

        public GetProductRepository(IDapperWrapper dapperWrapper, string defaultImageLink)
        {
            _dapperWrapper = dapperWrapper;
            _defaultImageLink = defaultImageLink;
        }

        public async Task<DomainProductResponse> GetBySearchString(string productSearchString)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ProductSearchString", productSearchString);

            var databaseProduct = (await _dapperWrapper.ExecuteStoredProcedure<DatabaseProduct>("GetProduct", parameters)).FirstOrDefault();

            if (databaseProduct is null)
            {
                throw new NotFoundException("Product not found.");
            }

            if (string.IsNullOrEmpty(databaseProduct.Name)) 
            {
                throw new NullReferenceException("Product name may not be empty.");
            }

            var domainProduct = new DomainProductResponse(); 

            if (string.IsNullOrEmpty(databaseProduct.ImageLink))
            {
                domainProduct.ImageLink = _defaultImageLink;
            }
            else
            {
                domainProduct.ImageLink = databaseProduct.ImageLink;
            }

            domainProduct.Name = databaseProduct.Name;
            domainProduct.Price = databaseProduct.Price;

            return domainProduct;
        }
    }
}
