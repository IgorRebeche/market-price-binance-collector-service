using Application.Services.BinanceService;
using Application.UseCases.CollectTickersUseCase;
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
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<ICollectTickersUseCase, CollectTickersUseCase>();
            return services;
        }
    }
}
