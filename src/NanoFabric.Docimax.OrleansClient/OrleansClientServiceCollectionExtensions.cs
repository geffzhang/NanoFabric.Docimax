using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NanoFabric.Docimax.OrleansClient;
using NanoFabric.Docimax.OrleansClient.AccessToken;
using Orleans.Runtime;
using System;


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
            services.AddSingletonNamedService<IAccessTokenService, ClientAccessTokenService>((AccessTokenType.ClientCredentials.ToString()));
            services.AddSingletonNamedService<IAccessTokenService, UserAccessTokenService>((AccessTokenType.UserCredentials.ToString()));
            services.AddSingletonNamedService<IAccessTokenService, OrleansContextTokenService>((AccessTokenType.OrleansContext.ToString()));
            return services;
        }

       
    }
}
