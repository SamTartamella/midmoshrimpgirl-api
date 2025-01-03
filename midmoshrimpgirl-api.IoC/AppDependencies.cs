using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using midmoshrimpgirl_api.dataAccess.Repositories;
using midmoshrimpgirl_api.dataAccess.Wrappers.Dapper;
using midmoshrimpgirl_domain.DataAccess;
using midmoshrimpgirl_domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midmoshrimpgirl_api.IoC
{
    public static class AppDependencies
    {
        public static void RegisterDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IGetProduct, GetProduct>();
            services.AddScoped<IGetProductRepository>((s) => new GetProductRepository(
                s.GetService<IDapperWrapper>(),
                config["DefaultImage"]
                ));
            services.AddScoped<IDapperWrapper>((s) => new DapperWrapper(
                config["ConnectionStrings:MidMoShrimpGirlDB"]));
        }
    }
}
