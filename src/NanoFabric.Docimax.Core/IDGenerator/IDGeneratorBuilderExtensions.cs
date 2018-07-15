using Microsoft.Extensions.DependencyInjection;
using NanoFabric.Docimax.Core.IDGenerator.Snowflake;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Docimax.Core.IDGenerator
{
    public static class IDGeneratorBuilderExtensions
    {
        /// <summary>
        /// Add IIDGenerated Service
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIDGenerator(this IServiceCollection services)
        {
            services.AddSingleton<IWorkerOpation, WorkerOpation>();
            services.AddSingleton<IIDGenerated, IdWorker>();
            return services;
        }
    }
}
