using Microsoft.Extensions.DependencyInjection;
using Services.Contracts;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class ServiceRegister
    {
        public static IServiceCollection RegisterService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
