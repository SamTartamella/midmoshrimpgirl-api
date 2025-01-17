using midmoshrimpgirl_domain.Models;

namespace midmoshrimpgirl_domain.Queries
{
    public interface IGetProduct
    {
        Task<DomainProductResponse> WithSearchString(string productSearchString);

        Task<DomainProductResponse> WithName(string productName);
    }
}
