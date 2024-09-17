using DataAccess.Contracts;
using DataAccess.DataContext;
using DataAccess.UOW;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class DataAccessRegister
    {
        public static IServiceCollection RegisterDataAccess(this IServiceCollection services)
        {

            services.AddDbContext<AppDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
