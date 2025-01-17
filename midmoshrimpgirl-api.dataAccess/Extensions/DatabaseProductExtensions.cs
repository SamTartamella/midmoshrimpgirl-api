using midmoshrimpgirl_api.dataAccess.Models;
using midmoshrimpgirl_domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midmoshrimpgirl_api.dataAccess.Extensions;
public static class DatabaseProductExtensions
{
    public static DomainProductResponse ToDomainProduct(this DatabaseProduct databaseProduct)
    {
        var domainProduct = new DomainProductResponse();
        domainProduct.Name = databaseProduct.Name;
        domainProduct.Price = databaseProduct.Price;
        domainProduct.ImageLink = databaseProduct.ImageLink;
        return domainProduct;   
    }
}
