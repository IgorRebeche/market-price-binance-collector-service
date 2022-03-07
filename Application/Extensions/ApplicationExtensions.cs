using Application.Services.BinanceService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static void AddApplicationExtensions(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IBinanceService, BinanceService>();
        }
    }
}
