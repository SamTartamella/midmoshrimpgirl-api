using Dapper;
using midmoshrimpgirl_api.dataAccess.Models;
using midmoshrimpgirl_api.dataAccess.Wrappers.Dapper;
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
