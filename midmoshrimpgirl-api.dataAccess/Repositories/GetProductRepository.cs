using Dapper;
using midmoshrimpgirl_api.dataAccess.Extensions;
using midmoshrimpgirl_api.dataAccess.Models;
using midmoshrimpgirl_api.dataAccess.Wrappers.Dapper;
using midmoshrimpgirl_domain.DataAccess;
using midmoshrimpgirl_domain.Models;
using midmoshrimpgirl_domain.Models.Exceptions;
using NullReferenceException = midmoshrimpgirl_domain.Models.Exceptions.NullReferenceException;

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

            var databaseProduct = (await _dapperWrapper.ExecuteStoredProcedure<DatabaseProduct>("GetProduct", parameters))
                .FirstOrDefault();

            if (databaseProduct is null)
            {
                throw new NotFoundException("Product not found.");
            }

            if (string.IsNullOrEmpty(databaseProduct.Name))
            {
                throw new NullReferenceException("Product name is empty.");
            }

            var domainProduct = databaseProduct.ToDomainProduct();

            if (string.IsNullOrEmpty(databaseProduct.ImageLink))
            {
                domainProduct.ImageLink = _defaultImageLink;
            }

            return domainProduct;
        }

        public async Task<DomainProductResponse> GetByName(string productName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ProductName", productName);

            var databaseProduct = (await _dapperWrapper.ExecuteStoredProcedure<DatabaseProduct>("GetProductByName", parameters))
                .FirstOrDefault();

            if (databaseProduct is null)
            {
                throw new NotFoundException("Product not found.");
            }

            var domainProduct = databaseProduct.ToDomainProduct();

            if (string.IsNullOrEmpty(databaseProduct.ImageLink))
            {
                domainProduct.ImageLink = _defaultImageLink;
            }

            return domainProduct;

        }
    }
}
