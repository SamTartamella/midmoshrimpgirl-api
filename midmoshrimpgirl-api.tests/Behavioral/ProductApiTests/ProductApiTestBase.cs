using midmoshrimpgirl_api.Controllers;
using midmoshrimpgirl_api.dataAccess.Repositories;
using midmoshrimpgirl_api.dataAccess.Wrappers.Dapper;
using midmoshrimpgirl_domain.DataAccess;
using midmoshrimpgirl_domain.Queries;
using NSubstitute;
using RandomTestValues;

namespace midmoshrimpgirl_api.tests.Behavioral.ProductApiTests;
public class ProductApiTestBase
{
    protected IDapperWrapper _dapperWrapperMock;
    protected IGetProduct _getProduct;
    protected IGetProductRepository _getProductRepository;
    protected string _mockDefaultImageLink;
    protected ProductApi _sut;

    [TestInitialize]
    public virtual void Init()
    {
        _mockDefaultImageLink = RandomValue.String();
        _dapperWrapperMock = Substitute.For<IDapperWrapper>();
        _getProductRepository = new GetProductRepository(_dapperWrapperMock, _mockDefaultImageLink);
        _getProduct = new GetProduct(_getProductRepository);

        _sut = new ProductApi(_getProduct);
    }
}
