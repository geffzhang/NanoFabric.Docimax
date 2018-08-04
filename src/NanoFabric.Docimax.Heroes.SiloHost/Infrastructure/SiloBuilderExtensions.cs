using Microsoft.Extensions.DependencyInjection;
using NanoFabric.Docimax.Core;
using NanoFabric.Docimax.Core.Orleans;
using NanoFabric.Docimax.Core.Utils;
using Orleans.Authentication;
using Orleans.Authentication.IdentityServer4;
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
        public static ISiloHostBuilder UseHeroConfiguration(this ISiloHostBuilder siloHost, IAppInfo appInfo, HostingEnvironment hostingEnv)
        {
            var siloAddress = IPAddress.Loopback;
            var siloPort = 11111;
            var gatewayPort = 30000;

            siloHost
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "heroes";
                })
                .AddMemoryGrainStorage(OrleansConstants.GrainPersistenceStorage)
                .UseConsulClustering(options => {
                    options.Address = new Uri("http://127.0.0.1:8500");
                })
                .ConfigureEndpoints(siloAddress, siloPort, gatewayPort)
                .AddAuthentication((HostBuilderContext context, AuthenticationBuilder authen) =>
                {
                    authen.AddIdentityServerAuthentication(opt =>
                    {
                        opt.RequireHttpsMetadata = true;
                        opt.Authority = "http://192.168.0.104:50774";
                        opt.ApiName = "DocimaxHeros";
                    });
                }, IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddAuthorizationFilter(); 

            return siloHost;
        }

    }
}
