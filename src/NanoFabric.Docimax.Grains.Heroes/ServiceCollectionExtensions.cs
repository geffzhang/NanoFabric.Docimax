using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Docimax.Grains.Heroes
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHeroesGrains(this IServiceCollection services)
        {
            services.AddSingleton<IHeroDataClient, MockHeroDataClient>();
            return services;
        }
    }
}
