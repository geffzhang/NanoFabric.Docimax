using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NanoFabric.Docimax.OrleansClient;
using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class OrleansClientServiceCollectionExtensions
    {
        /// <summary>
        /// Add Orleans Client Services
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns></returns>
        public static IServiceCollection AddOrleansClient(this IServiceCollection services, Action<IOrleansClientBuilder> builder)
        {
            builder.Invoke(new OrleansClientBuilder(services));
            services.AddSingleton<IOrleansClient, OrleansClient>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton(typeof(IKeyedServiceCollection<,>), typeof(KeyedServiceCollection<,>));
            return services;
        }

       
    }
}
