using Microsoft.Extensions.Configuration;
using Orleans;
using System;


namespace NanoFabric.Docimax.OrleansClient
{
    /// <summary>
    /// OrleansClient Builder
    /// </summary>
    public interface IOrleansClientBuilder
    {
        /// <summary>
        /// Add Orleans Client
        /// </summary>
        /// <param name="options">Config</param>
        /// <param name="builder"></param>
        /// <returns></returns>
        IOrleansClientBuilder AddClient(Action<OrleansClientOptions> options, Action<IClientBuilder> builder = null);

        /// <summary>
        /// Add Orleans Client
        /// </summary>
        /// <param name="configuration">Config</param>
        /// <param name="builder"></param>
        /// <returns></returns>
        IOrleansClientBuilder AddClient(IConfiguration configuration, Action<IClientBuilder> builder = null);     
        

    }
}
