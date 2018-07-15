using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NanoFabric.Docimax.Core;
using NanoFabric.Docimax.Grains.Contracts.Heroes;
using NanoFabric.Docimax.Heroes.Api.Infrastructure;
using NanoFabric.Docimax.Heroes.Api.Realtime;
using NanoFabric.Docimax.Heroes.Client;
using Orleans;

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

            var clientBuilderContext = new ClientBuilderContext
            {
                Configuration = Configuration,
                AppInfo = appInfo,
                ConfigureClientBuilder = clientbuilder =>
                    clientbuilder.ConfigureApplicationParts(x => x.AddApplicationPart(typeof(IHeroCollectionGrain).Assembly).WithReferences())
                    .UseSignalR()
            };

            services.AddSignalR()
                .AddOrleans();

            services.UseOrleansClient(clientBuilderContext);
            services.AddHeroesClients();
            services.AddMvc();
            services.AddCors(o => o.AddPolicy("TempCorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,IHostingEnvironment env, ILoggerFactory loggerFactory	)
		{
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();

                app.UseCors("TempCorsPolicy");

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseSignalR(routes =>
                {
                    routes.MapHub<HeroHub>("/realtime/hero");
                    routes.MapHub<UserNotificationHub>("/userNotifications");
                });

                app.UseMvc();
            }
    }
}
