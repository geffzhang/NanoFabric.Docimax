using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NanoFabric.AspNetCore;
using NanoFabric.Docimax.Core;
using NanoFabric.Docimax.Core.Utils;
using NanoFabric.Docimax.Grains.Contracts.Heroes;
using NanoFabric.Docimax.Heroes.Api.Infrastructure;
using NanoFabric.Docimax.Heroes.Api.Realtime;
using NanoFabric.Docimax.Heroes.Client;
using NanoFabric.Docimax.OrleansClient;
using Ocelot.JwtAuthorize;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace NanoFabric.Docimax.Heroes.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var appInfo = new AppInfo(Configuration);
            services.AddSingleton<IAppInfo>(appInfo);

            //var clientBuilderContext = new ClientBuilderContext
            //{
            //    Configuration = Configuration,
            //    AppInfo = appInfo,
            //    ConfigureClientBuilder = clientbuilder =>
            //        clientbuilder.ConfigureApplicationParts(x => x.AddApplicationPart(typeof(IHeroCollectionGrain).Assembly).WithReferences())
            //        .UseSignalR()
            //};      
            //services.UseOrleansClient(clientBuilderContext);

            services.AddNanoFabricConsul(Configuration);
            services.AddOrleansClient(build =>
            {
                build.AddClient(Configuration.GetSection("OrleansClient:Heroes"), builder =>
                {
                    var c = Configuration.GetSection("OrleansClient:Heroes").Get<OrleansClientOptions>();
                    builder.ConfigureApplicationParts(x => x.AddApplicationPart(typeof(IHeroCollectionGrain).Assembly).WithReferences())
                    .UseSignalR();
                });              
            });

            //var client = InitializeWithRetries(clientBuilder).Result;
            //services.AddSingleton(client);
            //services.AddSignalR().AddOrleans(new SignalRClusterClientProvider(client));

            services.AddHeroesClients();
            services.AddMvc();
            services.AddCors(o => o.AddPolicy("TempCorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            //services.AddApiJwtAuthorize((context) =>
            //{
            //    return ValidatePermission(context);
            //});
        }


        private static async Task<IClusterClient> InitializeWithRetries(IClusterClient clientConfig)
        {
            var attempt = 0;
            var stopwatch = Stopwatch.StartNew();
            var clientClusterConfig = new ClientConfiguration();

            await Task.Delay(TimeSpan.FromSeconds(clientClusterConfig.DelayInitialConnectSeconds));

            var client = clientConfig;
            await client.Connect(async ex =>
            {
                attempt++;
                if (attempt > clientClusterConfig.ConnectionRetry.TotalRetries)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }

                var delay = RandomUtils.GenerateNumber(clientClusterConfig.ConnectionRetry.MinDelaySeconds, clientClusterConfig.ConnectionRetry.MaxDelaySeconds);
                Console.WriteLine("Client cluster failed to connect. Attempt {0} of {1}. Retrying in {2}s.", attempt, clientClusterConfig.ConnectionRetry.TotalRetries, delay);
                await Task.Delay(TimeSpan.FromSeconds(delay));
                return true;
            });

            Console.WriteLine("Client cluster connected successfully to silo in {0:#.##}s.", stopwatch.Elapsed.TotalSeconds);
            return client;
        }

        /// <summary>
        /// Cusomer Validate Method
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        bool ValidatePermission(HttpContext httpContext)
        {
            var permissions = new List<Permission>() {
                new Permission { Name="admin", Predicate="Get", Url="/api/values" },
                new Permission { Name="admin", Predicate="Post", Url="/api/values" }
            };
            var questUrl = httpContext.Request.Path.Value.ToLower();

            if (permissions != null && permissions.Where(w => w.Url.Contains("}") ? questUrl.Contains(w.Url.Split('{')[0]) : w.Url.ToLower() == questUrl && w.Predicate.ToLower() == httpContext.Request.Method.ToLower()).Count() > 0)
            {
                var roles = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Role).Value;
                var roleArr = roles.Split(',');
                var perCount = permissions.Where(w => roleArr.Contains(w.Name)).Count();
                if (perCount == 0)
                {
                    httpContext.Response.Headers.Add("error", "no permission");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILoggerFactory loggerFactory	)
		{
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();

                app.UseCors("TempCorsPolicy");

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                //app.UseSignalR(routes =>
                //{
                //    routes.MapHub<HeroHub>("/realtime/hero");
                //    routes.MapHub<UserNotificationHub>("/userNotifications");
                //});

                app.UseMvc()
                .UseConsulRegisterService(Configuration);
        }
    }
}
