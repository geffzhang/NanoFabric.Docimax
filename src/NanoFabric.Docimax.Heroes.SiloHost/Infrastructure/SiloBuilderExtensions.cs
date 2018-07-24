using Microsoft.Extensions.DependencyInjection;
using NanoFabric.Docimax.Core;
using NanoFabric.Docimax.Core.Orleans;
using NanoFabric.Docimax.Core.Utils;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace NanoFabric.Docimax.Heroes.SiloHost.Infrastructure
{
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Microsoft SQL Server invariant name string.
        /// </summary>
        public const string InvariantNameSqlServer = "System.Data.SqlClient";

        /// <summary>
        /// Oracle Database server invariant name string.
        /// </summary>
        public const string InvariantNameOracleDatabase = "Oracle.DataAccess.Client";

        /// <summary>
        /// SQLite invariant name string.
        /// </summary>
        public const string InvariantNameSqlLite = "System.Data.SQLite";

        /// <summary>
        /// MySql invariant name string.
        /// </summary>
        public const string InvariantNameMySql = "MySql.Data.MySqlClient";

        /// <summary>
        /// PostgreSQL invariant name string.
        /// </summary>
        public const string InvariantNamePostgreSql = "Npgsql";


        public static ISiloHostBuilder UseHeroConfiguration(this ISiloHostBuilder siloHost, IAppInfo appInfo, HostingEnvironment hostingEnv)
        {
              siloHost
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "Docimax.Heroes";
                });

            siloHost.UseAdoClustering(appInfo);

            return siloHost;
        }

        private static ISiloHostBuilder UseDevelopment(this ISiloHostBuilder siloHost)
        {
            siloHost
                .AddMemoryGrainStorage(OrleansConstants.PubSubStore)
                .ConfigureServices(services =>
                {
                    services.Configure<GrainCollectionOptions>(options => { options.CollectionAge = TimeSpan.FromMinutes(1.5); });
                })
                .Configure<ClusterMembershipOptions>(options => options.ExpectedClusterSize = 1);

            return siloHost;
        }


        private static ISiloHostBuilder UseAdoClustering(this ISiloHostBuilder siloHost, IAppInfo appInfo)
        {
            var siloAddress = IPAddress.Loopback;
            var siloPort = 11111;
            var gatewayPort = 30000;

            var invariant = InvariantNameSqlServer; // for Microsoft SQL Server
            string orleansConnectionString = "Data Source=120.132.116.226;Initial Catalog=Orleans;User Id=sa;Password =m54135ME;MultipleActiveResultSets=true";

            return siloHost.UseConsulClustering(options => {
                options.Address = new Uri(appInfo.ConsulEndPoint);
            })
                .UseAdoNetReminderService(opt =>
                {
                    opt.Invariant = invariant;
                    opt.ConnectionString = orleansConnectionString;
                })
                .AddAdoNetGrainStorageAsDefault(opt =>
                {
                    opt.ConnectionString = orleansConnectionString;
                    opt.Invariant = invariant;
                    opt.UseJsonFormat = true;
                })
                .ConfigureEndpoints(siloAddress, siloPort, gatewayPort, listenOnAnyHostAddress: true);
        }


    }
}
