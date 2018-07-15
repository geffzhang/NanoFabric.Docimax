using Microsoft.Extensions.DependencyInjection;
using NanoFabric.Docimax.Grains.Contracts.Heroes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Docimax.Heroes.Client
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHeroesClients(this IServiceCollection services)
        {
            services.AddScoped<IHeroClient, HeroClient>();
        }
    }
}
