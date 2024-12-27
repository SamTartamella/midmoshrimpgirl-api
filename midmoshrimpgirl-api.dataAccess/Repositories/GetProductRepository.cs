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

        public GetProductRepository(IDapperWrapper dapperWrapper)
        {
            _dapperWrapper = dapperWrapper;
        }

        public async Task<DomainProductResponse> GetById(int productId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", productId);

            var databaseProduct = (await _dapperWrapper.ExecuteStoredProcedure<DatabaseProduct>("GetProduct", parameters)).FirstOrDefault();

            if (databaseProduct is null)
            {
                throw new NotFoundException("Product not found.");
            }

            if (string.IsNullOrEmpty(databaseProduct.Name) || string.IsNullOrEmpty(databaseProduct.ImageLink)) 
            {
                throw new NullReferenceException("No product attributes may be empty.");
            }

            var domainProduct = new DomainProductResponse()
            {
                Name = databaseProduct.Name,
                Price = databaseProduct.Price,
                ImageLink = databaseProduct.ImageLink,
            };

            return domainProduct;
        }
    }
}
