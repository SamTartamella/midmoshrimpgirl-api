using midmoshrimpgirl_api.Models.Responses;
using midmoshrimpgirl_domain.Models;

namespace midmoshrimpgirl_api.Extensions;

public static class ProductResponseExtensions
{
    public static ProductResponse ToApiResponse(this DomainProductResponse source)
    {
        return new ProductResponse()
        {
            Name = source.Name,
            Price = source.Price,
            ImageLink = source.ImageLink
        };
    }
}
